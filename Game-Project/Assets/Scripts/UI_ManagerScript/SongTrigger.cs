using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip BossSong;
    private GameObject player;
    private AudioSource audioSource;
    private bool isPlayed;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = this.GetComponent<AudioSource>();
        isPlayed = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlayed)
        {
            audioSource.clip = BossSong;
            isPlayed = true;
            audioSource.Play();
        }
        
    }
}
