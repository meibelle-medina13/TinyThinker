using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    float time = 3600;
    void Update()
    {
        time -= Time.deltaTime;
        int hours = Mathf.FloorToInt(time / 3600);
        int mins = Mathf.FloorToInt(time / 60);
        int secs = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}",hours, mins, secs);
        //Debug.Log(time);
    }
}
