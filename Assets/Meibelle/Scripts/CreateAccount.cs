using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class CreateAccount : MonoBehaviour
{
    RectTransform rectTransform;

    [Header("<---- ERROR MESSAGE ---->")]
    [SerializeField]
    private GameObject errorMessage;

    [Header("<---- BACK BUTTON ---->")]
    [SerializeField]
    private GameObject back_button;

    [Header("<---- INPUT FIELDS ---->")]
    [SerializeField] 
    private TMP_InputField[] fields = new TMP_InputField[2];
    [SerializeField]
    private GameObject[] label = new GameObject[2];
    [SerializeField]
    private GameObject[] placeholder = new GameObject[2];
    [SerializeField]
    private GameObject[] text = new GameObject[2];

    private int index, prev;

    private void Start()
    {
        Button back = back_button.GetComponent<Button>();
        back.onClick.AddListener(() => GoToAccountSelection());
    }

    public void ShowLabel()
    {
        string selected = EventSystem.current.currentSelectedGameObject.name;
        if (selected == "Username")
        {
            index = 0;
        }
        else if (selected == "Relationship")
        {
            index = 1;
        }

        label[index].SetActive(true);
        placeholder[index].SetActive(false);

        rectTransform = text[index].GetComponent<RectTransform>();
        rectTransform.transform.localPosition = new Vector3(0, -4, 0);

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

    public void OnSubmitNewAccount()
    {
        string username = fields[0].text;
        string relationship = fields[1].text;


        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(relationship))
        {
            PlayerPrefs.SetString("Name", username);
            PlayerPrefs.SetString("Relationship", relationship);
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        }
        else
        {
            ShowErrorMessage();
        }
    }

    private void ShowErrorMessage()
    {
        errorMessage.SetActive(true);
    }

    public void RemoveErrorMessage()
    {
        errorMessage.SetActive(false);
    }

    private void GoToAccountSelection()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }
}
