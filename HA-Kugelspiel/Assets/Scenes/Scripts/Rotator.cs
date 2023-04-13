using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] public float anglesPerSecond = 180;
    float OriginalSpeed;
    [SerializeField] public int WaitTime = 0;
    bool isSlowed = false;
    // Update is called once per frame
    
    void Update()
    {
        transform.Rotate(Vector3.up, anglesPerSecond * Time.deltaTime, Space.World);
    }

    public void changeRotatorSpeed(float speed)
    {
        if(!isSlowed)
        {
            OriginalSpeed = anglesPerSecond;
            isSlowed = true;
        }else{
            StopCoroutine(revertRotatorSpeed());
        }
        StartCoroutine(revertRotatorSpeed());
        anglesPerSecond = speed;
        revertRotatorSpeed();

    }

    private IEnumerator revertRotatorSpeed()
    {
        yield return new WaitForSeconds(WaitTime);
        anglesPerSecond = OriginalSpeed;
        isSlowed = false;
    }
}
