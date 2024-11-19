using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Theme1_Map : MonoBehaviour
{
    [SerializeField]
    private GameObject[] theme1_levels = new GameObject[10];
    [SerializeField]
    private GameObject[] theme2_levels = new GameObject[10];

    [SerializeField]
    private GameObject[] themes = new GameObject[2];

    int current_theme, current_level, selected_theme;

    [SerializeField]
    private Button back_button;

    void Start()
    {
        back_button.onClick.AddListener(() => GoToMap());

        current_theme = PlayerPrefs.GetInt("Current_theme");
        current_level = PlayerPrefs.GetInt("Current_level");
        selected_theme = PlayerPrefs.GetInt("Selected Theme");

        for (int i = 0; i < theme1_levels.Length; i++)
        {
            Button level = theme1_levels[i].GetComponent<Button>();
            int index = i+1;
            level.onClick.AddListener(() => EnterGameLevel(index));
        }

        for (int i = 0; i < themes.Length; i++)
        {

            if (selected_theme == i+1)
            {
                themes[i].SetActive(true);
                if (current_theme > selected_theme)
                {
                    EnableAllLevels();
                }
                else
                {
                    EnableActiveLevels(current_level);
                }
            }
            else
            {
                themes[i].SetActive(false);
            }
        }


    }

    private void GoToMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }

    private void EnableAllLevels()
    { 
        for (int i = 0; i < 10; i++)
        {
            if (selected_theme == 1)
            {

                theme1_levels[i].SetActive(true);
            }
            else if (selected_theme == 2)
            {
                theme2_levels[i].SetActive(true);
            }
        }
    }

    private void EnableActiveLevels(int level)
    {
        for (int i = 0; i < level; i++)
        {
            if (current_theme == 1)
            {
                theme1_levels[i].SetActive(true);
            }
            else if (current_theme == 2)
            {
                theme2_levels[i].SetActive(true);
            }
        }
    }

    private void EnterGameLevel(int level)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(level + 6);
    }
}
