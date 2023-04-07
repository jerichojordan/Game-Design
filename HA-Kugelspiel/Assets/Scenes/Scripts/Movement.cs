using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 50f;

    private Rigidbody rb_;

    // Start is called before the first frame update
    void Start()
    {
        rb_ = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 fw = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        Vector3 mv = v * fw + h * right;
        mv.y = 0;
        mv.Normalize();

        mv = mv * speed;

        rb_.AddForce(mv);
    }
}
