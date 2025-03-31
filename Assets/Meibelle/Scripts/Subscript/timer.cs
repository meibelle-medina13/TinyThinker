using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    float time = 7200;
    //float time = 10;

  private void Start()
    {
        PlayerPrefs.DeleteKey("Time");
        if (PlayerPrefs.HasKey("Time"))
        {
            time = PlayerPrefs.GetFloat("Time");
        }
    }
    void Update()
    {
        time -= Time.deltaTime;
        int hours = Mathf.FloorToInt(time / 3600);
        int mins = Mathf.FloorToInt(time / 120);
        //int mins = Mathf.FloorToInt(time / 60);
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
