using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Theme1_Map : MonoBehaviour
{
    [SerializeField]
    private GameObject[] levels = new GameObject[10];
    int current_theme, current_level;
    void Start()
    {
        current_theme = PlayerPrefs.GetInt("Current_theme");
        current_level = PlayerPrefs.GetInt("Current_level");
        Debug.Log(current_level);

        if (current_theme > 1 )
        {
            EnableAllLevels();
        }
        else
        {
            if (current_level >= 1)
            {
                EnableActiveLevels(current_level);
            }
            else
            {
                levels[0].SetActive(true);
            }
        }

        for (int i = 0; i < levels.Length; i++)
        {
            Button level = levels[i].GetComponent<Button>();
            int index = i+1;
            level.onClick.AddListener(() => EnterGameLevel(index));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnableAllLevels()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(true);
        }
    }

    private void EnableActiveLevels(int level)
    {
        for (int i = 0; i < level; i++)
        {
            levels[i].SetActive(true);
        }
    }

    private void EnterGameLevel(int level)
    {
        //int sceneIndex = 6;
        //int sum = level + sceneIndex;
        //Debug.Log(sum);
        UnityEngine.SceneManagement.SceneManager.LoadScene(level + 6);
    }
}
