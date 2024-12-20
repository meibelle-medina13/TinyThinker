using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using static RESPONSE_CLASSES;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LEVEL_MAP_REQUESTS : MonoBehaviour
{
    private string URL = "https://tinythinker-server.up.railway.app";
    public UserRoot json;
    public VerificationRoot verificationJson;

    public IEnumerator GetUser(string endpoint, int user_id)
    {
        string newURL = URL + endpoint + "?ID=" + user_id;
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

    public IEnumerator VerifyBirthYear(string endpoint, string year, int guardian_id)
    {
        string newURL = URL + endpoint;
        WWWForm form = new WWWForm();
        form.AddField("ID", guardian_id);
        form.AddField("birth_year", year);

        using (UnityWebRequest www = UnityWebRequest.Post(newURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                verificationJson = JsonConvert.DeserializeObject<VerificationRoot>(www.downloadHandler.text);
            }
        }
    }
}
