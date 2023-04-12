using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class slowSpike : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private UnityEvent<float> slowAngleSpeed;
  


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            slowSpeed();
        }
        
    }
    private void slowSpeed()
    {
        slowAngleSpeed.Invoke(Speed);
    }

    



   
}
