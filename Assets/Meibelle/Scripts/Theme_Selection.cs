using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Theme_Selection : MonoBehaviour
{
    RectTransform rectTransform;

    [Header("<---- BUTTONS ---->")]
    [SerializeField]
    private Button select_button, prev_button, next_button;

    [Header("<---- BACK BUTTONS ---->")]
    [SerializeField]
    private Button showLogout;
    [SerializeField]
    private Button logout;

    [Header("<---- HIDE LOGOUT TIMELINE ---->")]
    [SerializeField]
    private GameObject hideLogout;

    [Header("<---- THEME CONTAINER ---->")]
    [SerializeField]
    private GameObject theme_container;

    private int x = 0;
    private int current_theme;

    private void Start()
    {
        current_theme = PlayerPrefs.GetInt("Current_theme");
        next_button.onClick.AddListener(() => Select_Theme("next"));
        prev_button.onClick.AddListener(() => Select_Theme("prev"));
        showLogout.onClick.AddListener(() => ManageLogout());
        CheckCurrentTheme();
    }

    private void ManageLogout()
    {
        if (logout.gameObject.activeSelf)
        {
            hideLogout.SetActive(true);
            StartCoroutine(HideLogout());
        }
        else
        {
            hideLogout.SetActive(false);
            logout.gameObject.SetActive(true);
            logout.onClick.AddListener(() => GoToAccSelection());
        }
    }

    IEnumerator HideLogout()
    {
        yield return new WaitForSeconds(0.5f);
        logout.gameObject.SetActive(false);
    }

    private void GoToAccSelection()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }

    private void Select_Theme(string action)
    {
        rectTransform = theme_container.GetComponent<RectTransform>();
        if (action == "next")
        {
            x += -964;
            rectTransform.transform.localPosition = new Vector3(-432 + x, 265.5f, 0);
            if (x  != -2892)
            {
                prev_button.interactable = true;
            }
            else
            {
                next_button.interactable = false;
            }
            select_button.onClick.RemoveAllListeners();
            CheckCurrentTheme();
        }
        else if (action == "prev")
        {
            x -= -964;
            rectTransform.transform.localPosition = new Vector3(-432 + x, 265.5f, 0);
            if (x < 0)
            {
                prev_button.interactable = true;
                next_button.interactable = true;
            }
            else
            {
                prev_button.interactable = false;
                next_button.interactable = true;
            }
            select_button.onClick.RemoveAllListeners();
            CheckCurrentTheme();
        }
    }

    private void GoToMap(int theme_num)
    {
        PlayerPrefs.SetInt("Selected_theme", theme_num);
        UnityEngine.SceneManagement.SceneManager.LoadScene(7);
    }

    private void CheckCurrentTheme()
    {
        rectTransform = theme_container.GetComponent<RectTransform>();
        select_button.interactable = true;
        if (rectTransform.transform.localPosition.x == -432 && current_theme >= 1)
        {
            select_button.onClick.AddListener(() => GoToMap(1));
        }
        else if (rectTransform.transform.localPosition.x == -1396 && current_theme >= 2)
        {
            select_button.onClick.AddListener(() => GoToMap(2));
        }
        else if (rectTransform.transform.localPosition.x == -2360 && current_theme >= 3)
        {
            select_button.onClick.AddListener(() => GoToMap(3));
        }
        else if (rectTransform.transform.localPosition.x == -3324 && current_theme == 4)
        {
            select_button.onClick.AddListener(() => GoToMap(4));
        }
        else
        {
            select_button.interactable = false;
        }
    }
}
