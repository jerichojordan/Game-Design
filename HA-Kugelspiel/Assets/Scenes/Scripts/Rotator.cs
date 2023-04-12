using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float anglesPerSecond = 180;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, anglesPerSecond * Time.deltaTime, Space.World);
    }

    public void changeRotatorSpeed(float speed)
    {
        anglesPerSecond = speed;
    }
}
