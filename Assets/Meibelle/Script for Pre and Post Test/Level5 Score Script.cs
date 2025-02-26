using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class Level5ScoreScript : MonoBehaviour
{
    int userID, delaytime;
    float Level5Score;

    private void Awake()
    {
        Level5Score = PlayerPrefs.GetFloat("Level5 Score");
        Debug.Log("FInal:" + Level5Score);
        userID = PlayerPrefs.GetInt("Current_user");
        StartCoroutine(GoToTest());
        //if (Level5Score >= 33.33f)
        //{
        //    StartCoroutine(GoToTest());
        //}
        //else
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene(7);
        //}
    }

    // -------------------------------------------------------------------- //

    public IEnumerator GoToTest()
    {
        delaytime = PlayerPrefs.GetInt("Delay Time");

        yield return new WaitForSeconds(delaytime);
        yield return StartCoroutine(UpdateCurrentScore());

        //Debug.Log("Go to map");
        //UnityEngine.SceneManagement.SceneManager.LoadScene(15);
    }

    float score;
    IEnumerator UpdateCurrentScore()
    {
        score = Level5Score * 100;
        userID = PlayerPrefs.GetInt("Current_user");
        int current_theme = PlayerPrefs.GetInt("Current_theme");
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"theme_num\": 1, \"level_num\": 5, \"score\": " + score + "}");

        //using (UnityWebRequest www = UnityWebRequest.Put("https://tinythinker-server.up.railway.app/scores", rawData))
        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/scores", rawData))
        {
            www.method = "PUT";
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Received: " + www.downloadHandler.text);
                Debug.Log("Theme" + current_theme);
                if (score >= 33.33f && current_theme == 1)
                {
                    PlayerPrefs.SetInt("Current_level", 0);
                    PlayerPrefs.SetString("PostTest Status", "Not yet done");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(15);
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(7);
                }
            }
        }
    }
}
