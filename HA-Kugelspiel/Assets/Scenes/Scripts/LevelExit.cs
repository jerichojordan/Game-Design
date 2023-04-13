using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private int LevelId;
    [SerializeField] private UnityEvent<int> levelExitTriggered;
    private ParticleSystem ps_;

    private void Start()
    {
        ps_ = GetComponentInChildren<ParticleSystem>();
        ps_.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelExitTriggered.Invoke(LevelId);
            ps_.Play();
        }
    }
}
