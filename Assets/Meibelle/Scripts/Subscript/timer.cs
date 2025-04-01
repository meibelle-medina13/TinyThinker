using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    float time = 0;
    int mins = 0;
    //float time = 10;

  private void Start()
    {
        if (PlayerPrefs.HasKey("Time"))
        {
            time = PlayerPrefs.GetFloat("Time");
        }
    }
    void Update()
    {
        time -= Time.deltaTime;
        int hours = Mathf.FloorToInt(time / 3600);
        if (hours >= 1)
        {
            mins = Mathf.FloorToInt(time / 120);
        }
        else
        {
            mins = Mathf.FloorToInt(time / 60);
        }

        int secs = Mathf.FloorToInt(time % 60);
        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}",hours, mins, secs);
        }

        PlayerPrefs.SetInt("Hours", hours);
        PlayerPrefs.SetInt("Mins", mins);
        PlayerPrefs.SetInt("Secs", secs);
        PlayerPrefs.SetFloat("Time", time);
        
        if (time <= 0)
        {
            Debug.Log("Done");
            if (timerText != null)
            {
                timerText.text = string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
            }
            PlayerPrefs.SetFloat("Time", time);
        }
    }
}
