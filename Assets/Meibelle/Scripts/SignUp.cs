using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class SignUp : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField]
    private GameObject turtle0;
    [SerializeField]
    private GameObject bear;
    [SerializeField]
    private GameObject turtle;

    [SerializeField]
    private GameObject[] inputField = new GameObject[4];
    [SerializeField]
    private GameObject[] label = new GameObject[4];
    [SerializeField]
    private GameObject[] placeholder = new GameObject[4];
    [SerializeField]
    private GameObject[] text = new GameObject[4];

    [SerializeField]
    private GameObject[] Panels = new GameObject[3];

    [SerializeField]
    private GameObject errormessage;

    int index, prev, counter, fieldCount;
    bool result;

    string defaultErrorMessage;

    string URL = "http://localhost:3000/users_guardian";

    void Start()
    {
        StartCoroutine(DelayNotice());
        StartCoroutine(Get());

        for (int i = 0; i < Panels.Length; i++)
        {
            int index = i;
            Button nextButton = Panels[i].GetComponentInChildren<Button>();
            if (nextButton != null && nextButton.name == "Button")
            {
                nextButton.onClick.AddListener(() => OnContinue(index));
            }
        }

        defaultErrorMessage = errormessage.GetComponentInChildren<TMP_Text>().text;
    }

    IEnumerator DelayNotice()
    {
        yield return new WaitForSeconds(1.5f);
        Panels[0].SetActive(true);
        bear.SetActive(true);
        turtle.SetActive(true);
        turtle0.SetActive(false);
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
        else if (selected == "birthday")
        {
            index = 2;
        }
        else if (selected == "username")
        {
            index = 3;
        }

        label[index].SetActive(true);
        placeholder[index].SetActive(false);

        rectTransform = text[index].GetComponent<RectTransform>();
        rectTransform.transform.localPosition = new Vector3(0, -6, 0);

        if (prev != 0 && prev != 1 && prev != 2 && prev != 3)
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

    public void OnContinue(int index)
    {
        if (index == 0)
        {
            Panels[index].SetActive(false);
            Panels[index + 1].SetActive(true);
        }
        else if (index == 1)
        {
            bool isEmpty = IsEmptyInputField(3);
            if (isEmpty == false)
            {
                Panels[index].SetActive(false);
                Panels[index + 1].SetActive(true);
            }
            else
            {
                errormessage.SetActive(true);
            }
        }
        else if (index == 2)
        {
            bool isEmpty = IsEmptyInputField(1);
            if (isEmpty == false)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }
            else
            {
                errormessage.SetActive(true);
            }
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
                        email = field.GetComponent<TMP_InputField>().text;
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
                            if (2024 - birth_year < 18 || birth_month > 12 || birth_date > 31)
                            {
                                //note: age restriction error message //
                                errormessage.SetActive(true);
                                field.GetComponent<TMP_InputField>().text = "";
                                placeholder[3].SetActive(true);
                                fieldCount--;
                            }
                        }
                        else
                        {
                            errormessage.SetActive(true);
                            field.GetComponent<TMP_InputField>().text = "";
                            placeholder[3].SetActive(true);
                            fieldCount--;
                        }
                    }
                }
            }
            if (fieldCount == 3)
            {
                StartCoroutine(Upload("/users_guardian", email, password, birth_month, birth_date, birth_year));
                result = false;
            }
            else
            {
                fieldCount = 0;
                result = true;
            }
        }
        else if (fieldnum == 1)
        {
            if (!string.IsNullOrEmpty(inputField[3].GetComponent<TMP_InputField>().text))
            {
                result = false;
                string nickname = inputField[3].GetComponent<TMP_InputField>().text;
                PlayerPrefs.SetString("Name", nickname);
            }
            else
            {
                result = true;
            }
        }
        return result;
    }

    IEnumerator Upload(string endpoint, string email, string password, int birth_month, int birth_date, int birth_year)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("birth_month", birth_month);
        form.AddField("birth_date", birth_date);
        form.AddField("birth_year", birth_year);

        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Received: " + www.downloadHandler.text);
            }
        }
    }

    IEnumerator Get()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(URL))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Received: " + www.downloadHandler.text);
            }
        }
    }
    public void removeErrorMessage()
    {
        errormessage.SetActive(false);
    }
}