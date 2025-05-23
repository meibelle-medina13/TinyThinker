using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class EndLevelScoreScript : MonoBehaviour
{
    int userID, delaytime;
    float LevelScore;
    int theme, level;
    THEME1_LEVEL1_REQUESTS requestsManager;


    private void Awake()
    {
        userID = PlayerPrefs.GetInt("Current_user");
        //Level5Score = PlayerPrefs.GetFloat("Level5 Score");
        if (PlayerPrefs.HasKey("Theme1 Score"))
        {
            LevelScore = PlayerPrefs.GetFloat("Theme1 Score");
            theme = 1;
            level = 5;
            PlayerPrefs.DeleteKey("Theme1 Score");
        }
        else if (PlayerPrefs.HasKey("Theme2 Score"))
        {
          LevelScore = PlayerPrefs.GetFloat("Theme2 Score");
          theme = 2;
          level = 5;
          PlayerPrefs.DeleteKey("Theme2 Score");
        }
        else if (PlayerPrefs.HasKey("Theme3 Score"))
        {
            LevelScore = PlayerPrefs.GetFloat("Theme3 Score");
            theme = 3;
            level = 3;
            PlayerPrefs.DeleteKey("Theme3 Score");
        }
        else if (PlayerPrefs.HasKey("Theme4 Score"))
        {
            LevelScore = PlayerPrefs.GetFloat("Theme4 Score");
            theme = 4;
            level = 3;
            PlayerPrefs.DeleteKey("Theme4 Score");
        }
        Debug.Log("FInal:" + LevelScore);
        userID = PlayerPrefs.GetInt("Current_user");

        StartCoroutine(GoToTest());
    }

    // -------------------------------------------------------------------- //

    public IEnumerator GoToTest()
    {
        delaytime = PlayerPrefs.GetInt("Delay Time");
        Debug.Log(delaytime);
        yield return new WaitForSeconds(delaytime);

        if (PlayerPrefs.GetFloat(userID.ToString() + "Time") > 0)
        {
            yield return StartCoroutine(UpdateCurrentScore());
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(7);
        }
    }

    float score;
    IEnumerator UpdateCurrentScore()
    {
        score = LevelScore * 100;
        userID = PlayerPrefs.GetInt("Current_user");
        int current_theme = PlayerPrefs.GetInt("Current_theme");
        Debug.Log("TESTING: " + theme + ", " + level + ", " + userID);
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"theme_num\": " + theme + ", \"level_num\": " + level + ", \"score\": " + score + "}");

        using (UnityWebRequest www = UnityWebRequest.Put("https://tinythinker-server.up.railway.app/scores", rawData))
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
                if (theme == current_theme)
                {
                    if (!PlayerPrefs.HasKey(userID.ToString() + "PostTest Status" + theme.ToString()) && PlayerPrefs.GetFloat(userID.ToString() + "Time") > 0)
                    {
                        if (score >= 33.33f && current_theme == 1)
                        {
                            PlayerPrefs.SetString(userID.ToString() + "PostTest Status" + theme.ToString(), "Not yet done");
                            UnityEngine.SceneManagement.SceneManager.LoadScene(15);
                        }
                        else if (score >= 33.33f && current_theme == 2)
                        {
                            PlayerPrefs.SetString(userID.ToString() + "PostTest Status" + theme.ToString(), "Not yet done");
                            UnityEngine.SceneManagement.SceneManager.LoadScene(22);
                        }
                        else if (score >= 33.33f && current_theme == 3)
                        {
                            PlayerPrefs.SetString(userID.ToString() + "PostTest Status" + theme.ToString(), "Not yet done");
                            UnityEngine.SceneManagement.SceneManager.LoadScene(27);
                        }
                        else if (score >= 33.33f && current_theme == 4)
                        {
                            PlayerPrefs.SetString(userID.ToString() + "PostTest Status" + theme.ToString(), "Not yet done");
                            UnityEngine.SceneManagement.SceneManager.LoadScene(32);
                        }
                    }
                    else
                    {
                        if (score >= 33.33f && theme == 4)
                        {
                            if (PlayerPrefs.HasKey("User" + userID.ToString() + "Finished Game"))
                            {
                                UnityEngine.SceneManagement.SceneManager.LoadScene(33);
                            }
                        }
                        else
                        {
                            UnityEngine.SceneManagement.SceneManager.LoadScene(7);
                        }
                    }
                } 
                else
                {
                    if (theme == 4)
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(33);
                    }
                    else
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(7);
                    }
                }
            }
        }
    }
}
