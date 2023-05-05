using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour,IHittable
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float stopDistance = 12f;
    [SerializeField] private float backoffDistance = 7f;
    [SerializeField] private float HitPoint = 100f;
    [SerializeField] private float bulletForce = 40f;

    private Transform player;
    private GameObject player_rb;
    private Rigidbody2D rb;

    public float location;
    

    void Start()
    {
        player_rb = GameObject.FindGameObjectWithTag("Player");
        player = player_rb.transform;
        rb = this.GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        if (player_rb.GetComponent<Player>().location==location) {
            moveTowardPlayer();
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

    private void GetHit(RaycastHit2D hit)
    {
        HitPoint -= GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Damage;
        if(HitPoint <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void RecieveHit(RaycastHit2D hit)
    {
        GetHit(hit);
    }
}
