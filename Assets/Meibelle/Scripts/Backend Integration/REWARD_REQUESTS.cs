using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static RESPONSE_CLASSES;

public class REWARD_REQUESTS : MonoBehaviour
{
    private string URL = "https://tinythinker-server.up.railway.app";

    public RewardRoot json;

    public IEnumerator GetRewards(string endpoint, int userID)
    {
        string newURL = URL + endpoint + "?user_ID=" + userID;

        using (UnityWebRequest www = UnityWebRequest.Get(newURL))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                json = JsonConvert.DeserializeObject<RewardRoot>(www.downloadHandler.text);
                Debug.Log("JSON: " + json);
            }
        }
    }

    public IEnumerator AddReward(string endpoint, int user_ID, int reward_type_ID)
    {
        string newURL = URL + endpoint;
        WWWForm form = new WWWForm();
        form.AddField("user_ID", user_ID);
        form.AddField("reward_type_ID", reward_type_ID);

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
                UnityEngine.SceneManagement.SceneManager.LoadScene(5);
            }
        }
    }
}
