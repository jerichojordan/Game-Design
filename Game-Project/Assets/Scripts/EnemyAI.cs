using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour,IHittable
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float stopDistance = 12f;
    [SerializeField] private float backoffDistance = 7f;
    [SerializeField] private float HitPoint = 100f;
    [SerializeField] private Transform _gunPoint;
    [SerializeField] private GameObject _bulletTrail;
    [SerializeField] private GameObject _hitBlood;
    [SerializeField] private GameObject _deadBlood;
    [SerializeField] private float _weaponRange = 10f;
    [SerializeField] private AudioClip _gunShot;
    [SerializeField] private float bulletForce = 10f;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float accuracy = 0.8f;
    [SerializeField] private AudioClip firstcontact;
    [SerializeField] private AudioClip scream;
    [SerializeField] private AudioClip dead;
    [SerializeField] private float maxBullet;
    [SerializeField] private float reloadTime;
    [SerializeField] private AudioClip _reloadSound;
    private Transform player;
    private GameObject player_rb;
    private Rigidbody2D rb;
    private float shootTimer;
    public Animator animator;
    private float currentBullet;
    private GameObject flashtrigger;

    private bool isLocated;
    public float location;
    private bool isDead;
    private LevelFinish levelFinish;
    private bool first;
    private bool isReloading;
    

    //EnemyCounter
    private EnemyCounter enemyCounter;


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
    }

    void Update()
    {
        if (!isDead) {
            if (player_rb.GetComponent<Player>().location==location) {
                moveTowardPlayer();
                animator.SetFloat("Speed", Mathf.Abs(rb.velocity[0]) + Mathf.Abs(rb.velocity[1]));
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
        }
    }

    private void moveTowardPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player.position) < stopDistance && Vector2.Distance(transform.position, player.position) > backoffDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, player.position) < backoffDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        
        //Facing Sledge
        transform.right = new Vector2(player.position.x, player.position.y) - new Vector2(transform.position.x, transform.position.y);
    }

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
            if (hittable != null)
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
    private void deactivateBlood()
    {
        _hitBlood.gameObject.SetActive(false);
    }
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
