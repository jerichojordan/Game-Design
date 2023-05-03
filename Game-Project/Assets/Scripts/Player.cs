using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public Animator animator;

    [SerializeField] private float _speed = 2f;
    [SerializeField] private Transform _gunPoint;
    [SerializeField] private GameObject _bulletTrail;
    [SerializeField] private float _weaponRange = 10f;
    [SerializeField] private AudioClip _gunShot;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
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
                //var hittable = hit.collider.GetComponent<IHittable>;
                //hittable?.Hit();
            }
            else
            {
                var endPosition = _gunPoint.position + transform.right * _weaponRange;
                trailScript.setTargetPosition(endPosition);
            }
        }
    }
}
