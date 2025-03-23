using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;
using static RESPONSE_CLASSES;

public class THEME1_LEVEL1_REQUESTS : MonoBehaviour
{
    private string URL = "https://tinythinker-server.up.railway.app";

    public RewardRoot json;

    public IEnumerator UpdateCurrentLevel(string endpoint, int next_level, int userID)
    {
        string newURL = URL + endpoint;

        if (PlayerPrefs.GetInt("Current_level") < next_level && 
            PlayerPrefs.GetInt("Selected_theme") == PlayerPrefs.GetInt("Current_theme"))
        {
            byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"current_level\": " + next_level + "}");
            Debug.Log(userID + " : " + next_level);
            using (UnityWebRequest www = UnityWebRequest.Put(newURL, rawData))
            {
                www.method = "PUT";
                www.SetRequestHeader("Content-Type", "application/json");
                yield return www.SendWebRequest();
                Debug.Log(www.result);
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
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(7);
        }
    }

    public IEnumerator UpdateCurrentScore(string endpoint, float score, int userID, int theme_num, int level_num)
    {
        string newURL = URL + endpoint;
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"theme_num\": " + theme_num + ", \"level_num\": " + level_num + ", \"score\": " + score + "}");

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
                Debug.Log("Received: " + www.downloadHandler.text);
            }
        }
    }

    public IEnumerator AddReward(string endpoint, int user_ID, int reward_type_ID)
    {
        string newURL = URL + endpoint;
        WWWForm form = new WWWForm();
        form.AddField("user_ID", user_ID);
        form.AddField("reward_type_ID", reward_type_ID);

        Debug.Log("Add");

        using (UnityWebRequest www = UnityWebRequest.Post(newURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("New Reward Collected");
                PlayerPrefs.SetString(user_ID.ToString() +"-"+ reward_type_ID.ToString(), "True");
                Debug.Log("REWARD No.: " +PlayerPrefs.GetInt(user_ID.ToString() + "-" + reward_type_ID.ToString()));
            }
        }
    }
}
