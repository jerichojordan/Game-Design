using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IHittable
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Transform _gunPoint;
    [SerializeField] private GameObject _bulletTrail;
    [SerializeField] private float _weaponRange = 10f;
    [SerializeField] private AudioClip _gunShot;
    [SerializeField] private GameObject _CrosshairSprite;
    [SerializeField] private float HitPoint = 100f;

    public Animator animator;
    private float Damage = 35f;
    public float location;

    private Rigidbody2D _rigidbody;
    GameObject _Crosshair;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        var crossPos = _gunPoint.position;
        _Crosshair = Instantiate(
               _CrosshairSprite,
               crossPos,
               transform.rotation
               );
    }

    
    void Update()
    {
        CrosshairPositioning(_Crosshair);
        LookAtMouse();
        Move();
        Shoot();
    }

    //Function too make the player looking at the mouse cursor
    private void LookAtMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        transform.right =  mousePos - new Vector2(transform.position.x, transform.position.y);
    }

    private void Move()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _rigidbody.velocity = input.normalized * _speed;
        animator.SetFloat("speed", Mathf.Abs(_rigidbody.velocity[0]) + Mathf.Abs(_rigidbody.velocity[1]));
    }

    private void Shoot()
    {
        if(Input.GetMouseButtonDown(0))
        {
            AudioSource.PlayClipAtPoint(_gunShot, Camera.main.transform.position);
            animator.SetTrigger("Shoot");
            var hit = Physics2D.Raycast(
                _gunPoint.position,
                transform.right,
                _weaponRange
                );

            var trail = Instantiate(
                _bulletTrail,
                _gunPoint.position,
                transform.rotation
                );

            var trailScript = trail.GetComponent<BulletTrail>();
            if(hit.collider != null)
            {
                trailScript.setTargetPosition(hit.point);
                var hittable = hit.collider.GetComponent<IHittable>();
                if( hittable != null )
                {
                    hittable.RecieveHit(hit, Damage);
                }
            }
            else
            {
                var endPosition = _gunPoint.position + transform.right * _weaponRange;
                trailScript.setTargetPosition(endPosition);
            }
        }
    }
    private void CrosshairPositioning(GameObject Crosshair)
    {
        var hit = Physics2D.Raycast(
               _gunPoint.position,
               transform.right,
               10f
               );
        if (hit.collider != null)
        {
            Crosshair.transform.position = hit.point;
        }
        else
        {
            var pos = _gunPoint.position + transform.right*15f;
            Crosshair.transform.position = pos;
        }
    }

    private void GetHit(RaycastHit2D hit, float damage)
    {
        HitPoint -= damage;
        if (HitPoint <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void RecieveHit(RaycastHit2D hit, float damage)
    {
        GetHit(hit, damage);
    }
}

