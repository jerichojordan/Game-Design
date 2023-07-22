using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUI : MonoBehaviour
{

    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    private AudioSource hoverSource;
    
    // Start is called before the first frame update
    void Start()
    {
        hoverSource = GetComponent<AudioSource>();
        hoverSource.ignoreListenerPause = true;
    }

    public void playHoverSound()
    {
        if (hoverSource != null)
        {
            
            hoverSource.clip = hoverSound;
            hoverSource.Play();
            
        }
        
    }
    public void playClickSound()
    {
        if (hoverSource != null)
        {
            hoverSource.clip = clickSound;
            hoverSource.Play();
        }

    }
}
