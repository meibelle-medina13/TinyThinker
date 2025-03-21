using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static RESPONSE_CLASSES;

public class THEME_REQUEST : MonoBehaviour
{
    private string URL = "https://tinythinker-server.up.railway.app";

    public QuarterRoot json;

    public IEnumerator GetQuarterStatus(string endpoint)
    {
        string newURL = URL + endpoint;

        using (UnityWebRequest www = UnityWebRequest.Get(newURL))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                json = JsonConvert.DeserializeObject<QuarterRoot>(www.downloadHandler.text);
                Debug.Log("JSON: " + json);
            }
        }
    }
}
