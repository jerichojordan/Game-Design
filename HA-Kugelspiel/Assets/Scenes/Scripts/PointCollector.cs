using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PointCollector : MonoBehaviour
{
    [SerializeField] private UnityEvent<int> onPointPickup;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Points"))
        {
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
            Destroy(other.gameObject);
            int value = other.GetComponent<Point>().value;
            onPointPickup.Invoke(value);
        }
    }
}
