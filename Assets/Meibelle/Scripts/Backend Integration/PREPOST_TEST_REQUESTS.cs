using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PREPOST_TEST_REQUESTS : MonoBehaviour
{
    private string URL = "https://tinythinker-server.up.railway.app";

    public IEnumerator updateTestScore(string endpoint, int userID, int theme, int testType, int score)
    {
        string newURL = URL + endpoint;

        byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"theme_num\": " + theme + ", \"test_type\": " + testType + ", \"score\": " + score + "}");

        using (UnityWebRequest www = UnityWebRequest.Put(newURL, rawData))
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
                if (theme == 4 && testType == 2)
                {
                    Debug.Log("Received: " + www.downloadHandler.text);
                    PlayerPrefs.SetString("PostTest Status", "Done");
                    PlayerPrefs.SetString("User"+ userID.ToString() + "Finished Game", "True");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(33);
                }
                else
                {
                    PlayerPrefs.SetInt("Current_level", 1);
                    Debug.Log("Received: " + www.downloadHandler.text);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(7);
                }
            }
        }
    }

    public IEnumerator UpdateCurrentTheme(string endpoint, int userID, int theme_num)
    {
        string newURL = URL + endpoint;
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"current_theme\": " + theme_num + "}");

        using (UnityWebRequest www = UnityWebRequest.Put(newURL, rawData))
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
                UnityEngine.SceneManagement.SceneManager.LoadScene(6);
                PlayerPrefs.SetInt("Current_theme", theme_num);
                Debug.Log("Received: " + www.downloadHandler.text);
                PlayerPrefs.SetString("PostTest Status", "Done");
            }
        }
    }

    public IEnumerator UpdateCurrentLevel(string endpoint, int next_level, int userID)
    {
        string newURL = URL + endpoint;
        byte[] rawData;
        if (next_level == 0)
        {
            next_level.ToString();
            rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"current_level\": \"" + next_level + "\"}");
        }
        else
        {
            rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"current_level\": " + next_level + "}");
        }
        using (UnityWebRequest www = UnityWebRequest.Put(newURL, rawData))
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
                PlayerPrefs.SetInt("Current_level", next_level);
                Debug.Log("Received: " + www.downloadHandler.text);
                UnityEngine.SceneManagement.SceneManager.LoadScene(7);
            }
        }
    }
}
