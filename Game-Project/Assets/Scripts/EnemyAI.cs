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
    [SerializeField] private float _weaponRange = 10f;
    [SerializeField] private AudioClip _gunShot;
    [SerializeField] private float bulletForce = 40f;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float accuracy = 0.8f;
    [SerializeField] private float maxAmmo = 12f;
    [SerializeField] private AudioClip _reloadSound;
    [SerializeField] private float reloadTime;
    [SerializeField] private float minimalDist = 3f;

    private Transform player;
    private GameObject player_rb;
    private Rigidbody2D rb;
    private float shootTimer;
    private bool isReloading;
    private float currentAmmo;

    public Animator animator;
    public float location;
    private bool isDead;
    private GameManager gameManager;

    //EnemyCounter
    private EnemyCounter enemyCounter;


    void Start()
    {
        isDead = false;
        player_rb = GameObject.FindGameObjectWithTag("Player");
        player = player_rb.transform;
        rb = this.GetComponent<Rigidbody2D>();
        shootTimer = shootInterval;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        currentAmmo = maxAmmo;
        isReloading = false;

        //EnemyCounter
        enemyCounter = GameObject.FindObjectOfType<EnemyCounter>();


    }

    void Update()
    {
        if (!isDead) {
            if (player_rb.GetComponent<Player>().location==location && Vector3.Distance(player_rb.transform.position,this.transform.position)<=minimalDist) {
                moveTowardPlayer();
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0f && currentAmmo > 0)
                {
                    Shoot();
                    shootTimer = shootInterval; // Reset the timer
                }else if(currentAmmo <= 0 && !isReloading)
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
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity[0]) + Mathf.Abs(rb.velocity[1]));
        //Facing Sledge
        transform.right = new Vector2(player.position.x, player.position.y) - new Vector2(transform.position.x, transform.position.y);
    }

    private void GetHit(RaycastHit2D hit,float damage)
    {
        HitPoint -= damage;
        if(HitPoint <= 0)
        {
            isDead= true;
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            Destroy(this.GetComponent<Rigidbody2D>());
            Destroy(this.GetComponent<CircleCollider2D>());
            animator.SetBool("isDead", true);
            gameManager.enemyKilledInc();
            //EnemyCounter
            enemyCounter.EnemyKilled();
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
        currentAmmo--;
        
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
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reload complete!");
    }
}
