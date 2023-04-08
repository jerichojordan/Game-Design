using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent buttonTriggered;

    private void OnTriggerEnter(Collider other)
    {
        buttonTriggered.Invoke();
    }
}
