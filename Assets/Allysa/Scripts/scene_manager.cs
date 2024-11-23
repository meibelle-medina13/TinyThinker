using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public List<TextMeshProUGUI> textWithOutline;
    public int counter = 1;
    public List<GameObject> scenes;
    public AudioSource Background_music1;
    public AudioSource Background_music2;
    //public GameObject assessment;
    //public GameObject achievementboard;

    //activate button
    [Header("Activate Buttons")]

    [Header("Scene 8")]
    [Header("Buttons to activate")]
    public List<Button> SCENE8_activate_buttons;
    [Header("Buttons to deactivate")]
    public List<GameObject> SCENE8_deactivate_buttons;

    [Header("Scene 9")]
    public Button Activate_button;

    [Header("Assessment")]
    public List<GameObject> clappingHandsAnimation;
    public List<Button> clappingHandsButton;
    public int answered_wrong;
    public Image result;

    [Header("Achievement Board")]
    public List<GameObject> star_display;
    public List<GameObject> confetti_size;
    public TextMeshProUGUI complimentary_text;
    public GameObject original_complimentBoard1;
    public GameObject originalStar_background1;
    public GameObject zeroStar_background1;
    public GameObject zeroStar_complimentBoard1;

    [Header("Next Button")]
    public Button nextButton;

    void Start()
    {
        Background_music1.Play();
        textWithOutline[0].fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.3f);
        textWithOutline[0].fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
    }

    void Update()
    {

        if (counter == 2)
        {
            nextButton.gameObject.SetActive(true);
        }

        else if (counter == 3)
        {
            nextButton.gameObject.SetActive(false);
        }

        else if (counter == 4)
        {
            nextButton.gameObject.SetActive(true);
        }

        else if (counter == 7)
        {
            textWithOutline[1].fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.40f);
            textWithOutline[1].fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.white);
        }

        else if (counter == 11)
        {
            if (Background_music1.isPlaying)
            {
                Background_music1.Stop();
            }
            if (!Background_music2.isPlaying)
            {
                Background_music2.Play();
            }
        }

    }

    public void whenButtonClicked()
    {
        if (counter <= 10)
        {
            if (counter == 6)
            {
                nextButton.gameObject.SetActive(false);
                UpdateScene();
            }

            else if (counter == 9)
            {
                UpdateScene();
                nextButton.gameObject.SetActive(false);
            }

            else if (counter == 10)
            {
                UpdateScene();
                Show_Stars();
            }

            else
            {
                UpdateScene();
            }
        }
    }

    public void UpdateScene()
    {
        scenes[counter].SetActive(false);
        counter++;
        scenes[counter].SetActive(true);
    }

    public void updateButton_state()
    {
        nextButton.gameObject.SetActive(true);
    }

    public void OnAnimationEnd()
    {
        if (counter == 0 || counter == 1)
        {
            UpdateScene();
        }

        else if (counter == 7 )
        {
            foreach (Button button in SCENE8_activate_buttons)
            {
                button.gameObject.SetActive(true);
            }

            foreach (GameObject deact_button in SCENE8_deactivate_buttons)
            {
                deact_button.gameObject.SetActive(false);
            }
        }

        else
        {
            nextButton.gameObject.SetActive(true);
        }
    }

    public void deactivate_buttons()
    {
        foreach (Button button in SCENE8_activate_buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void clappingHand_state()
    {
        foreach (GameObject gameobject in clappingHandsAnimation)
        {
            gameobject.SetActive(false);
        }
        
        foreach (Button button in clappingHandsButton)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void Disable_clicking()
    {
        foreach (Button button in clappingHandsButton)
        {
            button.interactable = false;
        }
    }

    public void activate_padlockButton()
    {
        Activate_button.gameObject.SetActive(true);
    }

    public void increment_wrongAnswer()
    {
        answered_wrong++;
    }

    public void assessmentResult()
    {
        if (answered_wrong == 0)
        {
            result.fillAmount = Mathf.Clamp01(0.99f);
        }
        else if (answered_wrong == 1)
        {
            result.fillAmount = Mathf.Clamp01(0.66f);
        }
        else if (answered_wrong == 2)
        {
            result.fillAmount = Mathf.Clamp01(0.33f);
        }
        else
        {
            result.fillAmount = Mathf.Clamp01(0.139f);
        }
    }

    public void ReloadCurrentScene()
    {
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
    }

    void Show_Stars()
    {
        nextButton.gameObject.SetActive(false);

        if (result.fillAmount < 0.33f)
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

        else if (Mathf.Approximately(result.fillAmount, 0.33f))
        {
            star_display[1].SetActive(true);
            confetti_size[1].SetActive(false);
            complimentary_text.text = "SUBOK";
        }
        else if (Mathf.Approximately(result.fillAmount, 0.66f))
        {
            star_display[2].SetActive(true);
            complimentary_text.text = "MAGALING";
        }
        else if (Mathf.Approximately(result.fillAmount, 0.99f))
        {
            star_display[3].SetActive(true);
            complimentary_text.text = "PERPEKTO";
        }
    }
}
