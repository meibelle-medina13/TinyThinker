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
    private Button generalExit;
    [SerializeField]
    private Slider speakerVolume;
    [SerializeField]
    private Slider micVolume;


    [Header("<---- GAME EXIT ---->")]
    [SerializeField]
    private Button[] exit = new Button[2];

    [Header("<---- AUDIO SOURCE ---->")]
    [SerializeField]
    private AudioSource bgMusic;
    [SerializeField]
    private AudioSource voiceOver;
    [SerializeField]
    private AudioSource SFX;

    [Header("<---- AUDIO CLIP ---->")]
    [SerializeField]
    private AudioClip buttonClick;

    void Start()
    {
        if (PlayerPrefs.HasKey("SpeakerVolume"))
        {
            LoadVolume();
        }
        else
        {
            PlayerPrefs.SetFloat("SpeakerVolume", 0.47f);
            PlayerPrefs.SetFloat("AudioVolume", 1f);
            LoadVolume();
        }


        PlayerPrefs.SetString("Paused", "False");

        if (gameObject.name != "Game Settings Manager")
        {
            menu_buttons[0].onClick.AddListener(() => OpenMenuPanel(0));
            for (int i = 0; i < paused.Length; i++)
            {
                int index = i;
                paused[index].onClick.AddListener(() => PausedFunctions(index));
            }
        }
        
        menu_buttons[1].onClick.AddListener(() => OpenMenuPanel(1));

        generalExit.onClick.AddListener(() => { OpenMenuPanel(2); });

        for (int i = 0; i < exit.Length; i++)
        {
            int index = i;
            exit[index].onClick.AddListener(() => ExitFunctions(index));
        }
    }

    private void OpenMenuPanel(int index)
    {
        Debug.Log("clicked");
        SFX.PlayOneShot(buttonClick);

        if (menu_panels[index].activeSelf)
        {
            menu_panels[index].SetActive(false);
            PlayerPrefs.SetString("Paused", "False");
        }
        else
        {
            PlayerPrefs.SetString("Paused", "True");
            if (gameObject.name != "Game Settings Manager")
            {
                if (index == 0)
                {
                    menu_panels[1].SetActive(false);
                }
                else
                {
                    menu_panels[0].SetActive(false);
                }
            }
            menu_panels[index].SetActive(true);
        }
    }

    private void PausedFunctions(int index)
    {
        SFX.PlayOneShot(buttonClick);

        if (index == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
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

    private void ExitFunctions(int index)
    {
        SFX.PlayOneShot(buttonClick);

        if (index == 0)
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene(5);
            Application.Quit();
        }
        else
        {
            menu_panels[2].SetActive(false);
        }
    }

    public void ChangeVolume(string type)
    { 
        if (gameObject.name == "Game Settings Manager")
        {
            bgMusic = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
            Debug.Log(bgMusic.name);
        }

        if (type == "Speaker")
        {
            bgMusic.volume = speakerVolume.value;
            PlayerPrefs.SetFloat("SpeakerVolume", speakerVolume.value);
        }
        else if (type == "Mic")
        {
            if (micVolume.value <= 0.25f)
            {
                micVolume.value = 0.25f;
            }

            //voiceOver.volume = micVolume.value;
            PlayerPrefs.SetFloat("AudioVolume", micVolume.value);
            if (gameObject.name != "Game Settings Manager")
            {
                voiceOver.volume = micVolume.value;
            }
        }
    }

    private void LoadVolume()
    {
        speakerVolume.value = PlayerPrefs.GetFloat("SpeakerVolume");
        micVolume.value = PlayerPrefs.GetFloat("AudioVolume");

        bgMusic.volume = speakerVolume.value;
        if (gameObject.name != "Game Settings Manager")
        {
            voiceOver.volume = micVolume.value;
        }
    }
}
