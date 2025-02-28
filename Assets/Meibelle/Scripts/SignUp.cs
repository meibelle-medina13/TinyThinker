using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class SignUp : MonoBehaviour
{
    RectTransform rectTransform;

    [Header("<---- CHARACTER BEHIND ---->")]
    [SerializeField]
    private GameObject turtle0;

    [Header("<---- INPUT FIELDS ---->")]
    [SerializeField]
    private GameObject[] inputField = new GameObject[5];
    [SerializeField]
    private GameObject[] label = new GameObject[5];
    [SerializeField]
    private GameObject[] placeholder = new GameObject[5];
    [SerializeField]
    private GameObject[] text = new GameObject[5];

    [Header("<---- SCENE PANELS ---->")]
    [SerializeField]
    private GameObject[] Panels = new GameObject[3];

    [Header("<---- BACK BUTTONS ---->")]
    [SerializeField]
    private Button[] backButton = new Button[2];

    [Header("<---- ERROR MESSAGE ---->")]
    [SerializeField]
    private GameObject errormessage;

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private SIGNUP_LOGIN_REQUESTS requestsManager;

    private int index, prev, counter, fieldCount, duplicate;
    private bool result;
    private string message;

    void Start()
    {
        requestsManager = FindObjectOfType<SIGNUP_LOGIN_REQUESTS>();

        StartCoroutine(DelayNotice());

        for (int i = 0; i < Panels.Length; i++)
        {
            int index = i;
            Button button = Panels[i].GetComponentInChildren<Button>();
            if (button != null && button.name == "Button")
            {
                button.onClick.AddListener(() => OnContinue(index));
            }
        }

        for (int i = 0; i < backButton.Length; i++)
        {
            int index = i;
            backButton[i].onClick.AddListener(() => GoBack(index));
        }

    }

    IEnumerator DelayNotice()
    {
        yield return new WaitForSeconds(1);
        Panels[0].SetActive(true);
        turtle0.SetActive(false);
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
        else if (selected == "birthday")
        {
            index = 2;
        }
        else if (selected == "username")
        {
            index = 3;
        }
        else if (selected == "relationship")
        {
            index = 4;
        }

        label[index].SetActive(true);
        placeholder[index].SetActive(false);

        rectTransform = text[index].GetComponent<RectTransform>();
        rectTransform.transform.localPosition = new Vector3(0, -6, 0);

        if (prev != 0 && prev != 1 && prev != 2 && prev != 3 && prev != 4)
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

    private void GoBack(int index)
    {
        if (index == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        else if (index >= 1)
        {
            Panels[index].SetActive(false);
            Panels[index - 1].SetActive(true);
        }
    }

    public void OnContinue(int index)
    {
        if (index == 0)
        {
            Panels[index].SetActive(false);
            Panels[index + 1].SetActive(true);
        }
        else if (index == 1)
        {
            StartCoroutine(ValidateInputs());
        }
        else if (index == 2)
        {
            bool isEmpty = IsEmptyInputField(2);
            if (isEmpty == false)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(3);
            }
        }
    }

    IEnumerator ValidateInputs()
    {
        if (inputField[0].GetComponent<TMP_InputField>().text != "")
        {
            yield return StartCoroutine(requestsManager.CheckEmailDuplicates("/users_guardian/guardianID", inputField[0].GetComponent<TMP_InputField>().text));
        }

        bool isEmpty = IsEmptyInputField(3);
        if (isEmpty == false)
        {
            Panels[1].SetActive(false);
            Panels[2].SetActive(true);
        }
    }

    private bool IsEmptyInputField(int fieldnum)
    {
        string email = "";
        string password = "";
        int birth_month = 0;
        int birth_date = 0;
        int birth_year = 0;

        if (fieldnum == 3)
        {
            foreach (GameObject field in inputField)
            {
                if (!string.IsNullOrEmpty(field.GetComponent<TMP_InputField>().text))
                {
                    fieldCount++;
                    if (field.name == "email")
                    {
                        if (requestsManager.EmailHasDuplicates)
                        {
                            message = "Ang email na ibinigay ay nagamit na.";
                            field.GetComponent<TMP_InputField>().text = "";
                            placeholder[0].SetActive(true);
                            fieldCount--;
                            Debug.Log("email existing already");
                        }
                        else
                        {
                            email = field.GetComponent<TMP_InputField>().text;
                        }
                    }
                    else if (field.name == "password")
                    {
                        password = field.GetComponent<TMP_InputField>().text;
                    }
                    else if (field.name == "birthday")
                    {
                        Regex pattern = new Regex("(\\d{2})\\/(\\d{2})\\/(\\d{4})");
                        string input = field.GetComponent<TMP_InputField>().text;
                        if (pattern.IsMatch(input))
                        {
                            string[] birthday = input.Split('/');
                            int.TryParse(birthday[0], out birth_month);
                            int.TryParse(birthday[1], out birth_date);
                            int.TryParse(birthday[2], out birth_year);
                            if (2024 - birth_year < 10 || birth_month > 12 || birth_date > 31 || birth_year < 1950)
                            {
                                message = "Ang kaarawang ibinigay ay hindi maaaring tanggapin.";
                                field.GetComponent<TMP_InputField>().text = "";
                                placeholder[3].SetActive(true);
                                fieldCount--;
                            }
                        }
                        else
                        {
                            message = "Sundin ang tamang pormat ng kaarawan.";
                            field.GetComponent<TMP_InputField>().text = "";
                            placeholder[2].SetActive(true);
                            fieldCount--;
                        }
                    }
                }
            }
            if (fieldCount == 3)
            {
                StartCoroutine(requestsManager.AddGuardian("/users_guardian", email, password, birth_month, birth_date, birth_year));
                result = false;
            }
            else
            {
                ShowErrorMessage(message);
                fieldCount = 0;
                result = true;
                message = "";
            }
        }
        else if (fieldnum == 2)
        {
            if (!string.IsNullOrEmpty(inputField[3].GetComponent<TMP_InputField>().text) && !string.IsNullOrEmpty(inputField[4].GetComponent<TMP_InputField>().text))
            {
                result = false;
                string nickname = inputField[3].GetComponent<TMP_InputField>().text;
                PlayerPrefs.SetString("Name", nickname);

                string relationship = inputField[4].GetComponent<TMP_InputField>().text;
                PlayerPrefs.SetString("Relationship", relationship);
            }
            else
            {
                ShowErrorMessage(message);
                result = true;
                message = "";
            }
        }
        return result;
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
}