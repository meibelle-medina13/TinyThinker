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
    void Awake()
    {
        GameObject[] levels_BGM = GameObject.FindGameObjectsWithTag("Levels_BGM");
        //audioManager = FindObjectOfType<AudioManager>();
        if (currentGObject.activeSelf)
        {
            //audioManager.ChangeAudio(newAudio);
            levels_BGM[0].GetComponent<AudioManager>().ChangeAudio(newAudio);

        }
    }

}
