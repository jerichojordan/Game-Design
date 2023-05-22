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

    private Transform player;
    private GameObject player_rb;
    private Rigidbody2D rb;
    private float shootTimer;

    public float location;
    

    void Start()
    {
        player_rb = GameObject.FindGameObjectWithTag("Player");
        player = player_rb.transform;
        rb = this.GetComponent<Rigidbody2D>();
        shootTimer = shootInterval;
    }

    void Update()
    {
        if (player_rb.GetComponent<Player>().location==location) {
            moveTowardPlayer();
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                Shoot();
                shootTimer = shootInterval; // Reset the timer
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
        if(HitPoint <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void RecieveHit(RaycastHit2D hit, float damage)
    {
        GetHit(hit,damage);
    }

    private void Shoot()
    {

        var direction = new Vector2();
        direction.x = transform.right.x + Random.Range(-accuracy, accuracy);
        direction.y = transform.right.y + Random.Range(-accuracy, accuracy);

        AudioSource.PlayClipAtPoint(_gunShot, Camera.main.transform.position);
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
}
