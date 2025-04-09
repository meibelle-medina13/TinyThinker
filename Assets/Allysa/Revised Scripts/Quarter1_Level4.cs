using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Quarter1_Level4 : MonoBehaviour
{
    private static int scene_counter = 0;
    //private static bool bgMusicPlayed = false;

    //BUTTON
    private int Wrong_click = 0;

    private Audio_Manager audioManager2;

    public Image stars_bar;
    public Button NextScene_Button;

    public List<GameObject> scenes;
    public List<GameObject> timelines;
    public List<TextMeshProUGUI> text;
    public List<Button> clickableButtons;
    public List<GameObject> Image;
    public List<GameObject> clapsGameobjects;
    public List<GameObject> star_display;

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private THEME1_LEVEL1_REQUESTS requestsManager;

    [Header("<---- GAME MENU ---->")]
    [SerializeField]
    private GameObject gameMenu;

    void Start()
    {
        if (gameObject.name == "Level4 Scene Manager")
        {
            scene_counter = 0;
        }
        Debug.Log(scene_counter);
        requestsManager = FindObjectOfType<THEME1_LEVEL1_REQUESTS>();

        audioManager2 = FindObjectOfType<Audio_Manager>();

        //if (!bgMusicPlayed)
        //{
        //    if (audioManager2 != null)
        //    {
        //        audioManager2.scene_bgmusic(0.5f);
        //        bgMusicPlayed = true;
        //        audioManager2.audioSourceBG1.volume = 0.39f;
                
        //    }
        //}

        if (text[0] != null)
        {
            text[0].fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.3f);
            text[0].fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        }
    }

    void Update()
    {
        Image NextbuttonImageComponent = NextScene_Button.GetComponent<Image>();
        Color color = NextbuttonImageComponent.color;

        if (color.a != 1f)
        {
            NextScene_Button.interactable = false;
        }

        else
        {
            NextScene_Button.interactable = true;
        }

        if (scene_counter == 7)
        {
            text[1].fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.40f);
            text[1].fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.white);
        }

        else if (scene_counter == 9)
        {
            CancelInvoke("UpdateScene");
        }

        //else if (scene_counter == 11)
        //{
        //    audioManager2.Stop_backgroundMusic2();
        //    //Show_Stars();
        //}

        if (stars_bar.fillAmount >= 0.3333333333333333f && stars_bar.fillAmount < 0.6666666666666667f)
        {
            Image[6].SetActive(false);
        }

        else if (stars_bar.fillAmount >= 0.6666666666666667f && stars_bar.fillAmount < 1f)
        {
            Image[6].SetActive(false);
            Image[7].SetActive(false);
        }

        else if (Mathf.Approximately(stars_bar.fillAmount, 1f))
        {
            Image[6].SetActive(false);
            Image[7].SetActive(false);
            Image[8].SetActive(false);
        }

        int index = 0;

        if (gameObject.name == "Level4 Scene Manager")
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                if (scenes[i].activeSelf)
                {
                    index = i;
                }
            }

            if (index != 11)
            {
                PlayableDirector playableDirector;

                if (index >= 3)
                {
                    index -= 1;
                }

                Debug.Log("Panel" + index);
                playableDirector = timelines[index].GetComponent<PlayableDirector>();

                if (PlayerPrefs.GetString("Paused") == "True")
                {
                    playableDirector.Pause();
                }
                else
                {
                    playableDirector.Resume();
                }
            }
            else
            {
                gameMenu.SetActive(false);
            }


        }
    }

    public void UpdateScene()
    {
        scenes[scene_counter].SetActive(false);
        scene_counter++;
        scenes[scene_counter].SetActive(true);

        if (scene_counter == 3 || scene_counter == 7)
        {
            NextScene_Button.gameObject.SetActive(false);
        }

        else if (scene_counter == 4 || scene_counter == 9)
        {
            NextScene_Button.gameObject.SetActive(true);
        }

        else if (scene_counter == 10)
        {
            //audioManager2.assessment_bgmusic(0.5f);
            NextScene_Button.gameObject.SetActive(false);
        }
        // ------------------------------------------------------------------- //
        else if (scene_counter == 11)
        {
            Show_Stars();
        }
    }

    public void IncrementFillAmount(float amount)
    {
        stars_bar.fillAmount = Mathf.Clamp01(stars_bar.fillAmount + amount);
    }

    public void ButtonState()
    { 
        foreach (Button button in clickableButtons)
            {
                button.gameObject.SetActive(true);
            }

        foreach (GameObject gameobject in clapsGameobjects)
        {
            gameobject.SetActive(false);
        }
    }

    public void ButtonInitialState()
    {
        foreach (Button button in clickableButtons)
        {
            button.gameObject.SetActive(false);
        }

        foreach (GameObject gameobject in clapsGameobjects)
        {
            gameobject.SetActive(true);
        }
    }

    public void Interactable_Button()
    {
        foreach (Button clickable_object in clickableButtons)
        {
            clickable_object.interactable = true;
        }
    }

    public void CorrectButton(Button ClickedButton)
    {
        foreach (Button clickable_object in clickableButtons)
        {
            clickable_object.interactable = false;
        }

        if (ClickedButton.name == "clap_2")
        {
            Invoke("UpdateScene", 2);
        }

        else
        {
            if (Wrong_click == 0)
            {
                IncrementFillAmount(1f);
            }

            else if (Wrong_click == 1)
            {
                IncrementFillAmount(0.6666666666666667f);
            }

            else if (Wrong_click == 2)
            {
                IncrementFillAmount(0.3333333333333333f);
            }

            else
            {
                IncrementFillAmount(0.1666666666666667f);
            }

            Invoke("UpdateScene", 2);
        }
    }

    public void WrongButton()
    {
        Wrong_click++;
    }

    public void DelayUpdate()
    {
        Invoke("UpdateScene", 1f);
    }

    void Show_Stars()
    {
        NextScene_Button.gameObject.SetActive(false);

        float score = stars_bar.fillAmount * 100;
        int userID = PlayerPrefs.GetInt("Current_user");
        int theme_num = 1;
        int level_num = 4;
        int delaytime = 0;

        if (PlayerPrefs.GetFloat(userID.ToString() + "Time") > 0)
        {
            StartCoroutine(requestsManager.UpdateCurrentScore("/scores", score, userID, theme_num, level_num));
        }

        if (stars_bar.fillAmount < 0.3333333333333333f)
        {
            star_display[0].SetActive(true);
            Image[0].SetActive(false);
            Image[1].SetActive(false);
            Image[2].SetActive(true);
            Image[3].SetActive(true);
            Image[4].SetActive(false);
            Image[5].SetActive(false);
            delaytime = 8;
        }

        else if (stars_bar.fillAmount >= 0.3333333333333333f && stars_bar.fillAmount < 0.6666666666666667f)
        {
            star_display[1].SetActive(true);
            Image[0].SetActive(false);
            delaytime = 12;
        }

        else if (stars_bar.fillAmount >= 0.6666666666666667f && stars_bar.fillAmount < 1f)
        {
            star_display[2].SetActive(true);
            delaytime = 12;
        }

        else if (Mathf.Approximately(stars_bar.fillAmount, 1f))
        {
            star_display[3].SetActive(true);
            delaytime = 12;
        }

        StartCoroutine(GoToMap(score, userID, delaytime));
    }

    IEnumerator GoToMap(float score, int userID, int delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        if (score < (100f / 3f))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(7);
        }
        else
        {
            if (PlayerPrefs.GetFloat(userID.ToString() + "Time") > 0)
            {
                int next_level = 5;
                StartCoroutine(requestsManager.UpdateCurrentLevel("/users/updateLevel", next_level, userID));
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(7);
            }
        }
    }
}
