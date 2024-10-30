using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SignUp : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField]
    private GameObject notice;
    [SerializeField]
    private GameObject bear;
    [SerializeField]
    private GameObject turtle;

    [SerializeField]
    private GameObject[] inputField = new GameObject[4];
    [SerializeField]
    private GameObject[] label = new GameObject [4];
    [SerializeField]
    private GameObject[] placeholder = new GameObject[4];
    [SerializeField]
    private GameObject[] text = new GameObject[4];

    [SerializeField]
    private GameObject signup;
    [SerializeField]
    private GameObject username;
    [SerializeField]
    private GameObject errormessage;
    int index, prev, counter, fieldCount;
    bool result;

    void Start()
    {
        Invoke("DelayNotice", 0.5f);
    }

    private void DelayNotice()
    {
        notice.SetActive(true);
        bear.SetActive(true);
        turtle.SetActive(true);
    }

    public void showLabel()
    {
        foreach (GameObject item in inputField) 
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
            else if (selected == "relationship")
            {
                index = 2;
            }
            else if (selected == "username")
            {
                index = 3;
            }
        }

        label[index].SetActive(true);
        placeholder[index].SetActive(false);

        rectTransform = text[index].GetComponent<RectTransform>();
        rectTransform.transform.localPosition = new Vector3(0, -6, 0);
        Debug.Log(label[index].name);

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

    public void onContinue()
    {
        counter += 1;
        if (counter == 1)
        {
            notice.SetActive(false);
            signup.SetActive(true);
        }
        else if (counter == 2)
        {
            bool isEmpty = isEmptyInputField(3);
            Debug.Log(isEmpty);
            if (isEmpty == false)
            {
                signup.SetActive(false);
                username.SetActive(true);
            }
            else
            {
                counter -= 1;
                errormessage.SetActive(true);
            }
        }
        else if (counter == 3) 
        {
            Debug.Log(counter);
            bool isEmpty = isEmptyInputField(1);
            if (isEmpty == false)
            {
                Debug.Log("DONE!");
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }
            else
            {
                errormessage.SetActive(true);
            }
        }
    }

    private bool isEmptyInputField(int fieldnum)
    {
        if (fieldnum == 3)
        {
            foreach (GameObject field in inputField)
            {
            
                if (!string.IsNullOrEmpty(field.GetComponent<TMP_InputField>().text))
                {
                    Debug.Log(field.GetComponent<TMP_InputField>().text);
                    fieldCount++;
                }
            }
            if (fieldCount == 3)
            {
                result = false;
            }
            else
            {
                fieldCount = 0;
                result =  true;
            }
        } 
        else if (fieldnum == 1)
        {
            Debug.Log(string.IsNullOrEmpty(inputField[3].GetComponent<TMP_InputField>().text));
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

    public void removeErrorMessage()
    {
        errormessage.SetActive(false);
    }
}
