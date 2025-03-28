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
            //if (PlayerPrefs.HasKey("SpeakerVolume") && audioSource != null)
            //{
            //    audioSource.volume = PlayerPrefs.GetFloat("SpeakerVolume");
            //}
            //else
            //{
            //    audioSource.volume = 1.0f;
            //}
            audioSource.clip = audio;
            audioSource.Play();
        }
    }
}
