using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using static RESPONSE_CLASSES;
using UnityEngine.Networking;

public class SETUP_REQUESTS : MonoBehaviour
{
    //private string URL = "https://tinythinker-server.up.railway.app";
    private string URL = "http://localhost:3000";

    private int guardianID;
    public IEnumerator getGuardianID(string endpoint, string email)
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
                Debug.Log(json.data[0].ID);
            }
        }
    }

    public IEnumerator AddUser(string endpoint, string username, int age, string gender, string avatar_filename, int current_theme, int current_level, string relation_to_guardian)
    {
        string newURL = URL + endpoint;
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("age", age);
        form.AddField("gender", gender);
        form.AddField("avatar_filename", avatar_filename);
        form.AddField("current_theme", current_theme);
        form.AddField("current_level", current_level);
        form.AddField("relation_to_guardian", relation_to_guardian);
        form.AddField("guardian_ID", guardianID);

        using (UnityWebRequest www = UnityWebRequest.Post(newURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("New added user");
                UnityEngine.SceneManagement.SceneManager.LoadScene(5);
            }
        }
    }
}
