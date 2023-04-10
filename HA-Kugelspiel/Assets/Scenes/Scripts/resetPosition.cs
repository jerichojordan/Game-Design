using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetPosition : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField] private AudioClip resetSound;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spikes"))
        {
            AudioSource.PlayClipAtPoint(resetSound, Camera.main.transform.position);
            transform.position = startPosition;
        }
    }
}
