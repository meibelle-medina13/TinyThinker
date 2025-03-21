using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RESPONSE_CLASSES;
using UnityEngine.Networking;

public class STATISTICS_RESPONSES : MonoBehaviour
{
    private string URL = "https://tinythinker-server.up.railway.app";

    public ScoreRoot json;

    public IEnumerator GetLevelScores(string endpoint, int userID, int theme_num)
    {
        string newURL = URL + endpoint + "?user_ID=" + userID + "&theme_num=" + theme_num;

        using (UnityWebRequest www = UnityWebRequest.Get(newURL))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                json = JsonConvert.DeserializeObject<ScoreRoot>(www.downloadHandler.text);
                Debug.Log("JSON: "+json);
            }
        }
    }
}
