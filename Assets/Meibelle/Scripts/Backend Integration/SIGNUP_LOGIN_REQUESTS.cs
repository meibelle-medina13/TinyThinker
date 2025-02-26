using Newtonsoft.Json;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using static RESPONSE_CLASSES;

public class SIGNUP_LOGIN_REQUESTS : MonoBehaviour
{
    //private string URL = "https://tinythinker-server.up.railway.app";
    private string URL = "http://localhost:3000";

    public bool EmailHasDuplicates;
    public string errormessage;

    public IEnumerator Get(string endpoint)
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
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator CheckEmailDuplicates(string endpoint, string input)
    {
        string newURL = URL + endpoint + "?email=" + input;
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
                if (json.data.Count > 0)
                {
                    EmailHasDuplicates = true;
                }
                else
                {
                    EmailHasDuplicates = false;
                }
            }
        }
    }

    public IEnumerator AddGuardian(string endpoint, string email, string password, int birth_month, int birth_date, int birth_year)
    {
        string newURL = URL + endpoint;
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("birth_month", birth_month);
        form.AddField("birth_date", birth_date);
        form.AddField("birth_year", birth_year);

        using (UnityWebRequest www = UnityWebRequest.Post(newURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                PlayerPrefs.SetString("Email", email);
                Debug.Log("New Guardian Added");
            }
        }
    }

    public IEnumerator LoginGuardian(string endpoint, string email, string password)
    {
        string newURL = URL + endpoint;

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(newURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                VerificationRoot json = JsonConvert.DeserializeObject<VerificationRoot>(www.downloadHandler.text);
                if (json.data == "Login Successful!")
                {
                    PlayerPrefs.SetString("Email", email);
                    errormessage = "";
                    UnityEngine.SceneManagement.SceneManager.LoadScene(5);
                }
                else if (json.data == "Email Not Found!")
                {
                    errormessage = "Hindi pa nakaregister ang ibinigay na email.";
                }
                else
                {
                    errormessage = "Mali ang ibinigay na password.";
                }
            }
        }
    }
}
