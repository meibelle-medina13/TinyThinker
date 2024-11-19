using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Theme_Selection : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField]
    private Button select_button, prev_button, next_button;
    [SerializeField]
    private GameObject theme_container;
    [SerializeField]
    private GameObject[] themes = new GameObject[4];
    int x = 0;

    private void Start()
    {
        next_button.onClick.AddListener(() => Select_Theme("next"));
        prev_button.onClick.AddListener(() => Select_Theme("prev"));
        CheckCurrentTheme();
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
        PlayerPrefs.SetInt("Selected Theme", theme_num);
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);
    }

    private void CheckCurrentTheme()
    {
        rectTransform = theme_container.GetComponent<RectTransform>();
        if (rectTransform.transform.localPosition.x == -432)
        {
            select_button.onClick.AddListener(() => GoToMap(1));
        }
        else if (rectTransform.transform.localPosition.x == -1396)
        {
            select_button.onClick.AddListener(() => GoToMap(2));
        }
        else if (rectTransform.transform.localPosition.x == -2360)
        {
            select_button.onClick.AddListener(() => GoToMap(3));
        }
        else if (rectTransform.transform.localPosition.x == -3324)
        {
            select_button.onClick.AddListener(() => GoToMap(4));
        }
    }

}
