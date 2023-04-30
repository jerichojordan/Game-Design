using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    public Animator animator;
    
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        LookAtMouse();
        Move();
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
}
