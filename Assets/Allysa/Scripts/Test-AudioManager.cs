using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip backgroundMusic;
    [SerializeField]
    private AudioClip ClickSound;
    private static AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); 
        audioSource.clip = backgroundMusic;
        
        if (audioSource.clip != null) 
        { 
            audioSource.Play();
            audioSource.volume = 0.5f;
        }
    }

    public void Click()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = ClickSound;

        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
