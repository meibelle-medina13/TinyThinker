using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBGMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip newAudio;
    [SerializeField]
    private GameObject currentGObject;

    private AudioManager audioManager;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (currentGObject.activeSelf)
        {
            audioManager.ChangeAudio(newAudio);

        }
    }

}
