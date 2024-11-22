using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using static SelectAccount;

public class Login : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField]
    private GameObject[] panels = new GameObject[2];
    [SerializeField]
    private Button[] login_signup = new Button[2];

    [SerializeField]
    private GameObject[] inputField = new GameObject[2];
    [SerializeField]
    private GameObject[] label = new GameObject[2];
    [SerializeField]
    private GameObject[] placeholder = new GameObject[2];
    [SerializeField]
    private GameObject[] text = new GameObject[2];

    [SerializeField]
    private GameObject errormessage, login_button, back_button;

    int index, prev;

    string URL = "http://localhost:3000/users_guardian/login";

    private void Start()
    {
        Button login = login_button.GetComponent<Button>();
        login.onClick.AddListener(() => LogIn());

        Button back = back_button.GetComponent<Button>();
        back.onClick.AddListener(() => SwitchPanel("To choices"));

        login_signup[0].onClick.AddListener(() => SwitchPanel("To login"));
        login_signup[1].onClick.AddListener(() => SwitchPanel("To signup"));
    }

    private void SwitchPanel(string destination)
    {
        if (destination == "To choices")
        {
            panels[0].gameObject.SetActive(true);
            panels[1].gameObject.SetActive(false);
        }
        else if (destination == "To login")
        {
            panels[0].gameObject.SetActive(false);
            panels[1].gameObject.SetActive(true);
        }
        else if (destination == "To signup")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
    }

    public void showLabel()
    {
        string selected = EventSystem.current.currentSelectedGameObject.name;

        if (selected == "email")
        {
            index = 0;
        }
        else if (selected == "password")
        {
            index = 1;
        }

        label[index].SetActive(true);
        placeholder[index].SetActive(false);

        rectTransform = text[index].GetComponent<RectTransform>();
        rectTransform.transform.localPosition = new Vector3(0, -6, 0);

        if (prev != 0 && prev != 1)
        {
            prev = index;
        }
    }

    public void hideLabel()
    {
        label[index].SetActive(false);
        placeholder[index].SetActive(true);

        rectTransform = text[index].GetComponent<RectTransform>();
        rectTransform.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void LogIn()
    {
        string email = inputField[0].GetComponent<TMP_InputField>().text;
        string password = inputField[1].GetComponent<TMP_InputField>().text;

        if (email != "" && password != "")
        {
            StartCoroutine(LoginGuardian(email, password));
        }
        else
        {
            showErrorMessage("");
        }
    }

    private void showErrorMessage(string message)
    {
        if (message == null || message == "")
        {
            message = "Kumpletuhin ang mga detalye.";
        }
        string editedMessage = "PAALALA: " + message;
        print(editedMessage);
        errormessage.GetComponentInChildren<TMP_Text>().text = editedMessage;
        errormessage.SetActive(true);
        message = "";
    }

    public void removeErrorMessage()
    {
        errormessage.SetActive(false);
    }


    public class VerificationRoot
    {
        public bool success { get; set; }
        public string data { get; set; }
    }

    IEnumerator LoginGuardian(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
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
                    UnityEngine.SceneManagement.SceneManager.LoadScene(5);
                }
                else if (json.data == "Email Not Found!")
                {
                    showErrorMessage("Hindi pa nakaregister ang ibinigay na email.");
                }
                else
                {
                    showErrorMessage("Mali ang ibinigay na password.");
                }
            }
        }
    }
}
