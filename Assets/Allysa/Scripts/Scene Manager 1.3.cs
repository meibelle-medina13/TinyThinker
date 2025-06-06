using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;


public class Theme1Level3_SceneManager : MonoBehaviour
{

    


    public TextMeshProUGUI textWithOutline;
    public int counter = 0;
    public Image total_stars;
    public AudioSource Background_Music1;
    public AudioSource Background_Music2;


    ///DROP AND DRAG  
    public int placed_wrong = 0;

    public List<GameObject> scenes;

    [Header("Activity")]
    public GameObject Smallconfetti;
    public float correctPuzzle = 0;
    public float fixedPuzzle = 0;
    public List<Button> DisableInteractableACT;
    public AudioSource activity_audio;

    [Header("Assessment1")]
    public int placedObjects = 0;
    public List<Button> DisableInteractable1;
    public AudioSource assessment1_audio;

    [Header("Assessment2")]
    public Button[] buttons;
    public Button correctButton;
    public GameObject Glowing_Letter;
    public List<Button> DisableInteractable2;
    public AudioSource assessment2_audio;

    [Header("Assessment3")]
    public Button[] Audiobuttons;
    public Button correctAudioButton;
    public List<Button> DisableInteractable3;
    public AudioSource assessment3_audio;


    [Header("Achievement Board")]
    public List<GameObject> star_display;
    public List<GameObject> confetti_size;
    public TextMeshProUGUI complimentary_text;
    public GameObject original_complimentBoard1;
    public GameObject originalStar_background1;
    public GameObject zeroStar_background1;
    public GameObject zeroStar_complimentBoard1;

    [Header("Next Button")]
    public Button nextScene_Button;
    void Start()
    {
        textWithOutline.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.3f);
        textWithOutline.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black); 
        Background_Music1.Play();
    }

    void Update()
    {
        if (counter == 8)
        {
            if (Background_Music1.isPlaying)
            {
                Background_Music1.Stop();
            }
            if (!Background_Music2.isPlaying)
            {
                Background_Music2.Play();
            }
        }
    }

    public void whenButtonClicked()
    {
        if (counter == 3)
        {
            UpdateScene();
            UpdateButtonState(activity_audio, DisableInteractableACT);
            nextScene_Button.gameObject.SetActive(false);
        }

        else if (counter == 5)
        {
            UpdateScene();
            Smallconfetti.SetActive(false);
        }

        else if (counter == 6)
        {
            UpdateScene();
            nextScene_Button.gameObject.SetActive(false);
            counter++;
            scenes[counter].SetActive(true);
            UpdateButtonState(assessment1_audio, DisableInteractable1);
        }

        else if (counter == 8)
        {
            UpdateScene();
            nextScene_Button.gameObject.SetActive(false);
            UpdateButtonState(assessment2_audio, DisableInteractable2);
            placed_wrong = 0;
            Glowing_Letter.SetActive(false);

            foreach (Button button in buttons)
            {
                Button capturedButton = button;
                capturedButton.onClick.AddListener(() => CheckButton(capturedButton, correctButton));
            }
        }

        else if (counter == 9)
        {
            UpdateScene();
            nextScene_Button.gameObject.SetActive(false);
            UpdateButtonState(assessment3_audio, DisableInteractable3);
            placed_wrong = 0;

            foreach (Button button in Audiobuttons)
            {
                Button capturedAudioButton = button;
                capturedAudioButton.onClick.AddListener(() => CheckButton(capturedAudioButton, correctAudioButton));
            }
        }

        else if (counter == 10)
        {
            UpdateScene();
            scenes[7].SetActive(false);
            nextScene_Button.gameObject.SetActive(false);
            Show_Stars();

            if (Background_Music2.isPlaying)
            {
                Background_Music2.Stop();
            }
        }



        else
        {
            UpdateScene();
        }
    }

    public void IncrementPlacedCount()
    {
        placedObjects++;
        Debug.Log("Placed Objects: " + placedObjects);
        UpdateNextButtonState();
    }
    private void UpdateNextButtonState()
    {
        if (placedObjects == 3)
        {
            nextScene_Button.gameObject.SetActive(true);
        }
        else
        {
            nextScene_Button.gameObject.SetActive(false);
        }
    }

    private void UpdateScene()
    {
        scenes[counter].SetActive(false);
        counter++;
        scenes[counter].SetActive(true);
    }
    public void SetFillAmount(float amount)
    {
        total_stars.fillAmount = Mathf.Clamp01(amount);
    }

    public void IncrementFillAmount(float amount)
    {
        total_stars.fillAmount = Mathf.Clamp01(total_stars.fillAmount + amount);
    }

    private void CheckButton(Button Clicked, Button CorrectAnswer)
    {
        Debug.Log("Button clicked: " + Clicked.name);

        if (Clicked == CorrectAnswer)
        {
            if (counter == 9)
            {
                Glowing_Letter.SetActive(true);
            }

            if (placed_wrong == 0)
            {
                nextScene_Button.gameObject.SetActive(true);

                if (total_stars.fillAmount < 0.48f)
                {
                    IncrementFillAmount(0.16f);
                }

                else if (Mathf.Approximately(total_stars.fillAmount, 0.48f))
                {
                    SetFillAmount(0.72f);
                }

                else if (Mathf.Approximately(total_stars.fillAmount, 0.72f))
                {
                    SetFillAmount(1f);
                }
            }
            else
            {
                nextScene_Button.gameObject.SetActive(true);
            }
        }

        else
        {
            placed_wrong++;
        }
    }

    // ------------------------------------------------------- //
    int delaytime;
    // ------------------------------------------------------- //
    void Show_Stars()
    {
        nextScene_Button.gameObject.SetActive(false);
        // ------------------------------ //
        StartCoroutine(UpdateCurrentScore());
        // ------------------------------ //
        if (total_stars.fillAmount < 0.48f)
        {
            star_display[0].SetActive(true);
            confetti_size[0].SetActive(false);
            confetti_size[1].SetActive(false);
            zeroStar_background1.SetActive(true);
            zeroStar_complimentBoard1.SetActive(true);
            original_complimentBoard1.SetActive(false);
            originalStar_background1.SetActive(false);
            complimentary_text.text = "ULITIN!";
        }

        else if (Mathf.Approximately(total_stars.fillAmount, 0.48f))
        {
            star_display[1].SetActive(true);
            confetti_size[1].SetActive(false);
            complimentary_text.text = "SUBOK";
            delaytime = 4;
        }
        else if (Mathf.Approximately(total_stars.fillAmount, 0.72f))
        {
            star_display[2].SetActive(true);
            complimentary_text.text = "MAGALING";
            delaytime = 4;
        }
        else if (Mathf.Approximately(total_stars.fillAmount, 1f))
        {
            star_display[3].SetActive(true);
            complimentary_text.text = "PERPEKTO";
            delaytime = 8;
        }

        if (total_stars.fillAmount >= 0.50f)
        {
            StartCoroutine(GoToMap());
        }
    }

    public void UpdateButtonState(AudioSource assessment_audio, List<Button> buttonsToDisable)
    {
        assessment_audio.Play();

        if (assessment_audio.isPlaying)
        {
            StartCoroutine(ReEnableButtonsAfterAudio(assessment_audio, buttonsToDisable));
        }
    }

    IEnumerator ReEnableButtonsAfterAudio(AudioSource audio, List<Button> DisableButtons)
    {
        foreach (Button button in DisableButtons)
        {
            button.interactable = false;
        }

        while (audio.isPlaying)
        {
            yield return null;
        }

        foreach (Button button in DisableButtons)
        {
            button.interactable = true; ;
        }
    }

    public void ReloadCurrentScene()
    {
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
    }

    // --------------------------------------------------- //

    int userID;
    float score;

    IEnumerator GoToMap()
    {
        yield return new WaitForSeconds(delaytime);
        StartCoroutine(UpdateCurrentLevel());
    }

    IEnumerator UpdateCurrentScore()
    {
        score = total_stars.fillAmount * 100;
        userID = PlayerPrefs.GetInt("Current_user");
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"theme_num\": 1, \"level_num\": 3, \"score\": " + score + "}");

        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/scores", rawData))
        {
            www.method = "PUT";
            www.SetRequestHeader("Content-Type", "application/json");
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

    IEnumerator UpdateCurrentLevel()
    {
        int current_level = 4;
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"current_level\": " + current_level + "}");

        if (score >= 50)
        {
            using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users", rawData))
            {
                www.method = "PUT";
                www.SetRequestHeader("Content-Type", "application/json");
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    PlayerPrefs.SetInt("Current_level", current_level);
                    Debug.Log("Received: " + www.downloadHandler.text);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(7);
                }
            }
        }

    }


}



////void deactivate_buttons()
////{
////    foreach (Button button in SCENE8_activate_buttons)
////    {
////        button.gameObject.SetActive(false);
////    }

////    foreach (Button button in ASSESSMENT_activate_buttons)
////    {
////        button.gameObject.SetActive(false);
////    }
////}


////    void master_counter9()
////{ 

////    }
////}
