using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour,IHittable
{
    [Header("Stats")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float HitPoint = 100f;
    [SerializeField] private float bulletForce = 10f;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float accuracy = 0.8f;
    [SerializeField] private float _weaponRange = 10f;
    [SerializeField] private float maxBullet;
    [SerializeField] private float reloadTime;
    [SerializeField] private float stopDistance = 12f;
    [SerializeField] private float backoffDistance = 7f;
    [Header("Audio")]
    [SerializeField] private AudioClip _gunShot;
    [SerializeField] private AudioClip firstcontact;
    [SerializeField] private AudioClip scream;
    [SerializeField] private AudioClip dead;
    [SerializeField] private AudioClip _reloadSound;
    [Header("Used Asset")]
    [SerializeField] private Transform _gunPoint;
    [SerializeField] private GameObject _bulletTrail;
    [SerializeField] private GameObject _hitBlood;
    [SerializeField] private GameObject _deadBlood;
    [SerializeField] private List<SteeringBehaviour> steeringBehaviours;
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private AIData aiData;
    [SerializeField] private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f;
    //Inputs sent from the Enemy AI to the Enemy controller
    [SerializeField] private Vector2 movementInput;
    [SerializeField] private ContextSolver movementDirectionSolver;

    public Animator animator;
    public float location;


    private bool isLocated;
    private bool isDead;
    private LevelFinish levelFinish;
    private bool first;
    private bool isReloading;
    private float currentBullet;
    private GameObject flashtrigger;
    private Transform player;
    private GameObject player_rb;
    private Rigidbody2D rb;
    private float shootTimer;
    //EnemyCounter
    private EnemyCounter enemyCounter;
    bool following = false;


    void Start()
    {
        isDead = false;
        isLocated = false;
        player_rb = GameObject.FindGameObjectWithTag("Player");
        player = player_rb.transform;
        rb = GetComponent<Rigidbody2D>();
        shootTimer = shootInterval;
        //EnemyCounter
        enemyCounter = GameObject.FindObjectOfType<EnemyCounter>();
        first = true;
        _hitBlood.gameObject.SetActive(false);
        _deadBlood.gameObject.SetActive(false);
        currentBullet = maxBullet;
        isReloading = false;
        flashtrigger = this.transform.Find("Light 2D + Trigger").gameObject;
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    

    void Update()
    {
        if (!isDead) {
            if (aiData.currentTarget != null) {
                moveTowardPlayer();
                if (following == false)
                {
                    following = true;
                    StartCoroutine(Chase());
                }
                animator.SetFloat("Speed", 1);
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0f && currentBullet >=0)
                {
                    if (!isLocated)
                    {
                        if (first == true)
                        {
                            AudioSource.PlayClipAtPoint(firstcontact, this.gameObject.transform.position);
                            first = false;
                        }
                        isLocated = true;
                    }
                    Shoot();
                    shootTimer = shootInterval; // Reset the timer
                    currentBullet--;
                }else if(currentBullet <=0)
                {
                    Reload();
                }
            }
            else if (aiData.GetTargetsCount() > 0)
            {
                //Target acquisition logic
                aiData.currentTarget = aiData.targets[0];
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
            moveTowardPlayer();
        }
    }
    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }


    /**
     * 
     * Enemy Movement
     * 
     **/
    private void moveTowardPlayer()
    {
        /*if (movementInput == Vector2.zero)
        {
            rb.velocity = movementInput * speed;
        }else if(Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            rb.velocity = movementInput * speed;
        }else if(Vector2.Distance(transform.position, player.position) < stopDistance && Vector2.Distance(transform.position, player.position) > backoffDistance)
        {
            rb.velocity = movementInput * 0;
        }else if (Vector2.Distance(transform.position, player.position) < backoffDistance)
        {
            rb.velocity = movementInput * speed;
        }*/
        rb.velocity = movementInput * speed;
        //Facing Sledge
        transform.right = new Vector2(aiData.currentTarget.position.x, aiData.currentTarget.position.y) - new Vector2(transform.position.x, transform.position.y);
        animator.SetFloat("speed", Mathf.Abs(rb.velocity[0]) + Mathf.Abs(rb.velocity[1]));
    }
    private IEnumerator Chase()
    {
        if (aiData.currentTarget == null)
        {
            //Stopping Logic
            Debug.Log("Stopping");
            movementInput = Vector2.zero;
            following = false;
            yield break;
        }
        else
        {
            //Chase logic
            movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
            yield return new WaitForSeconds(aiUpdateDelay);
            StartCoroutine(Chase());

        }

    }

    /**
     * 
     * Recieving Hit
     * 
     **/
    private void GetHit(RaycastHit2D hit,float damage)
    {
        HitPoint -= damage;
        if (HitPoint <= 0)
        {
            isDead= true;
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            AudioSource.PlayClipAtPoint(dead, this.gameObject.transform.position);
            Destroy(this.GetComponent<Rigidbody2D>());
            Destroy(this.GetComponent<CircleCollider2D>());
            animator.SetBool("isDead", true);
            _deadBlood.gameObject.SetActive(true);
            this.tag = "Untagged";
            flashtrigger.SetActive(false);
            //EnemyCounter
            enemyCounter.EnemyKilled();
        }
        else
        {
            AudioSource.PlayClipAtPoint(scream, this.gameObject.transform.position);
            _hitBlood.gameObject.SetActive(true);
            Invoke("deactivateBlood", 0.2f);
        }
    }
    public void RecieveHit(RaycastHit2D hit, float damage)
    {
        GetHit(hit,damage);
        Debug.Log("RecieveHit called");
    }

    /**
     * 
     * Shooting
     * 
     * */
    private void Shoot()
    {

        var direction = new Vector2();
        direction.x = player.position.x -(transform.position.x + Random.Range(-accuracy, accuracy));
        direction.y = player.position.y -(transform.position.y + Random.Range(-accuracy, accuracy));

        AudioSource.PlayClipAtPoint(_gunShot, transform.position);
        var hit = Physics2D.Raycast(
                _gunPoint.position,
                direction,
                _weaponRange
                 );

        var trail = Instantiate(
                _bulletTrail,
                _gunPoint.position,
                transform.rotation
                );

        var trailScript = trail.GetComponent<BulletTrail>();
        if (hit.collider != null)
        {
            trailScript.setTargetPosition(hit.point);
            var hittable = hit.collider.GetComponent<IHittable>();
            var hitted = hit.collider.GetComponent<Player>();
            if (hittable != null && hitted!=null)
            {
                    hittable.RecieveHit(hit,bulletForce);
            }
        }
        else
        {
            var endPosition = _gunPoint.position + transform.right * _weaponRange;
            trailScript.setTargetPosition(endPosition);
        }
        
    }

    /**
     * 
     * Blood animation get shot
     * 
     **/
    private void deactivateBlood()
    {
        _hitBlood.gameObject.SetActive(false);
    }


    /**
     * 
     * Reloading
     *
     **/
    private void Reload()
    {
        if (isReloading) return; // Ignore if already reloading
        AudioSource.PlayClipAtPoint(_reloadSound, transform.position);
        isReloading = true;
        Debug.Log("Reloading...");

        // Perform the reload logic
        // For example, you can play a reload animation or wait for a certain amount of time

        // After the reload time has passed
        Invoke("FinishReload", reloadTime);
    }

    private void FinishReload()
    {
        currentBullet = maxBullet;
        isReloading = false;
        AudioSource.PlayClipAtPoint(_reloadSound, transform.position);
        Debug.Log("Reload complete!");
    }
}
