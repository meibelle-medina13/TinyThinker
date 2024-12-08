using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    RectTransform rectTransform;

    [Header("<---- SCENE PANELS ---->")]
    [SerializeField]
    private GameObject[] panels = new GameObject[2];

    [Header("<---- SIGNUP/LOGIN BUTTONS ---->")]
    [SerializeField]
    private Button[] login_signup = new Button[2];

    [Header("<---- INPUT FIELDS ---->")]
    [SerializeField]
    private GameObject[] inputField = new GameObject[2];
    [SerializeField]
    private GameObject[] label = new GameObject[2];
    [SerializeField]
    private GameObject[] placeholder = new GameObject[2];
    [SerializeField]
    private GameObject[] text = new GameObject[2];

    [Header("<---- ERROR MESSAGE ---->")]
    [SerializeField]
    private GameObject errormessage;

    [Header("<---- BACK BUTTON ---->")]
    [SerializeField]
    private GameObject back_button;

    [Header("<---- LOGIN BUTTON ---->")]
    [SerializeField]
    private GameObject login_button;

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private SIGNUP_LOGIN_REQUESTS requestsManager;

    private int index, prev;

    private void Start()
    {
        requestsManager = FindObjectOfType<SIGNUP_LOGIN_REQUESTS>();

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
            panels[0].SetActive(true);
            panels[1].SetActive(false);
        }
        else if (destination == "To login")
        {
            panels[0].SetActive(false);
            panels[1].SetActive(true);
        }
        else if (destination == "To signup")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
    }

    public void ShowLabel()
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

    public void HideLabel()
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
            StartCoroutine(ValidateLogin(email, password));
        }
        else
        {
            ShowErrorMessage("");
        }
    }

    private void ShowErrorMessage(string message)
    {
        if (message == null || message == "")
        {
            message = "Kumpletuhin ang mga detalye.";
        }

        string editedMessage = "PAALALA: " + message;
        errormessage.GetComponentInChildren<TMP_Text>().text = editedMessage;
        errormessage.SetActive(true);
    }

    public void RemoveErrorMessage()
    {
        errormessage.SetActive(false);
    }

    IEnumerator ValidateLogin(string email, string password)
    {
        yield return StartCoroutine(requestsManager.LoginGuardian("/users_guardian/login", email, password));

        if (requestsManager.errormessage != "")
        {
            ShowErrorMessage(requestsManager.errormessage);
        }
    }
}
