using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    [Header("<---- GAME MENU ---->")]
    [SerializeField]
    private Button[] menu_buttons = new Button[2];
    [SerializeField]
    private GameObject[] menu_panels = new GameObject[3];

    [Header("<---- GAME PAUSED ---->")]
    [SerializeField]
    private Button[] paused = new Button[3];

    [Header("<---- GAME SETTINGS ---->")]
    [SerializeField]
    private Button[] settings = new Button[3];

    [Header("<---- GAME EXIT ---->")]
    [SerializeField]
    private Button[] exit = new Button[2];

    [Header("<---- SPRITES ---->")]
    [SerializeField]
    private Sprite[] mute = new Sprite[4];

    [Header("<---- AUDIO SOURCE ---->")]
    [SerializeField]
    private AudioSource bgMusic;
    [SerializeField]
    private AudioSource voiceOver;

    void Start()
    {
        bgMusic.volume = 0.47f;
        voiceOver.volume = 1;

        PlayerPrefs.SetString("Paused", "False");
        menu_buttons[0].onClick.AddListener(() => OpenMenuPanel(0));
        menu_buttons[1].onClick.AddListener(() => OpenMenuPanel(1));

        for (int i = 0; i < paused.Length; i++)
        {
            int index = i;
            paused[index].onClick.AddListener(() => PausedFunctions(index));
        }

        for (int i = 0; i < settings.Length; i++)
        {
            int index = i;
            settings[index].onClick.AddListener(() => SettingsFunctions(index));
        }

        for (int i = 0; i < exit.Length; i++)
        {
            int index = i;
            exit[index].onClick.AddListener(() => ExitFunctions(index));
        }
    }

    private void OpenMenuPanel(int index)
    {

        if (menu_panels[index].activeSelf)
        {
            menu_panels[index].SetActive(false);
            PlayerPrefs.SetString("Paused", "False");
        }
        else
        {
            PlayerPrefs.SetString("Paused", "True");
            if (index == 0)
            {
                menu_panels[1].SetActive(false);
            }
            else
            {
                menu_panels[0].SetActive(false);
            }
            menu_panels[index].SetActive(true);
        }
    }

    private void PausedFunctions(int index)
    {
        if (index == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        else if (index == 1)
        {
            PlayerPrefs.SetString("Paused", "False");
            menu_panels[0].SetActive(false);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(7);
        }
    }

    private void SettingsFunctions(int index)
    {
        if (index == 0)
        {
            Debug.Log(bgMusic.name);
            if (bgMusic.volume != 0)
            {
                bgMusic.volume = 0;
                settings[index].GetComponent<Image>().sprite = mute[0];
            }
            else
            {
                bgMusic.volume = 0.45f;
                settings[index].GetComponent<Image>().sprite = mute[2];
            }
        }
        else if (index == 1)
        {
            if (voiceOver.volume != 0)
            {
                voiceOver.volume = 0;
                settings[index].GetComponent<Image>().sprite = mute[1];
            }
            else
            {
                voiceOver.volume = 1;
                settings[index].GetComponent<Image>().sprite = mute[3];
            }
        }
        else
        {
            OpenMenuPanel(2);
        }
    }

    private void ExitFunctions(int index)
    {
        if (index == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(5);
        }
        else
        {
            menu_panels[2].SetActive(false);
        }
    }
}
