using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] int OpenTime=5;
    private Vector3 startPosition;
    private bool isOpen = false;

    private void Start()
    {
        startPosition = center.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        center.position = startPosition + new Vector3(0, 1.8f, 0);
        isOpen = true;
        Invoke(nameof(CloseDoor), OpenTime);
    }

    public void CloseDoor()
    {
        center.position = startPosition;
        isOpen = false;
    }
}
