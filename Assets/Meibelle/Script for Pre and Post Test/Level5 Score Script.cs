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
        StartCoroutine(UpdateCurrentScore());
        Debug.Log("FInal:" + Level5Score);
        userID = PlayerPrefs.GetInt("Current_user");
        StartCoroutine(GoToMap());
    }

    // -------------------------------------------------------------------- //

    public IEnumerator GoToMap()
    {
        yield return new WaitForSeconds(delaytime);
        Debug.Log("Go to map");
        StartCoroutine(UpdateCurrentLevel());
    }

    float score;
    IEnumerator UpdateCurrentScore()
    {
        score = Level5Score * 100;
        userID = PlayerPrefs.GetInt("Current_user");
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"theme_num\": 1, \"level_num\": 5, \"score\": " + score + "}");

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
            }
        }
    }

    IEnumerator UpdateCurrentLevel()
    {
        int current_level = 6;
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"current_level\": " + current_level + "}");

        if (score >= 33)
        {
            using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users", rawData))
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
                    PlayerPrefs.SetInt("Current_level", current_level);
                    Debug.Log("Received: " + www.downloadHandler.text);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(7);
                }
            }
        }

    }
}
