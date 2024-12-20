using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource audioSource;

    public void ChangeAudio(AudioClip audio)
    {
        audioSource.Stop();

        if (audio != null)
        {
            audioSource.clip = audio;
            audioSource.Play();
        }
    }
}
