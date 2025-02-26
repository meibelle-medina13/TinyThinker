using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using static RESPONSE_CLASSES;
using UnityEngine.Networking;

public class SELECT_ACCOUNT_REQUESTS : MonoBehaviour
{
    //private string URL = "https://tinythinker-server.up.railway.app";
    private string URL = "http://localhost:3000";

    public UserRoot json;
    public int guardianID;

    public IEnumerator GetGuardianID(string endpoint, string email)
    {
        string newURL = URL + endpoint + "?email=" + email;

        using (UnityWebRequest www = UnityWebRequest.Get(newURL))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Guardian_Root json = JsonConvert.DeserializeObject<Guardian_Root>(www.downloadHandler.text);
                guardianID = json.data[0].ID;
                PlayerPrefs.SetInt("Guardian_ID", guardianID);
            }
        }
    }

    public IEnumerator GetUsers(string endpoint, int guardianID)
    {
        string newURL = URL + endpoint + "?guardian_ID=" + guardianID;
        
        using (UnityWebRequest www = UnityWebRequest.Get(newURL))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                json = JsonConvert.DeserializeObject<UserRoot>(www.downloadHandler.text);
            }
        }
    }
}
