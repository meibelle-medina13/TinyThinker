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
    int userID;
    //float time = 10;

  private void Start()
    {
        userID = PlayerPrefs.GetInt("Current_user");
        if (PlayerPrefs.HasKey(userID.ToString() + "Time"))
        {
            time = PlayerPrefs.GetFloat(userID.ToString() + "Time");
        }
    }
    void Update()
    {
        time -= Time.deltaTime;
        int hours = Mathf.FloorToInt(time / 3600);
        mins = Mathf.FloorToInt((time % 3600) / 60);
        int secs = Mathf.FloorToInt(time % 60);
        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}",hours, mins, secs);
        }

        PlayerPrefs.SetFloat(userID.ToString() + "Time", time);
        
        if (time <= 0)
        {
            Debug.Log("Done");
            if (timerText != null)
            {
                timerText.text = string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
            }
            PlayerPrefs.SetFloat(userID.ToString() + "Time", time);
        }
    }
}
