using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Theme2Level4_SceneManager : MonoBehaviour
{
    public int scene_counter = 0;
    public int new_backgroundCounter = 0;
    public AudioSource background_music1; 
    public AudioSource background_music2; 
    public List<GameObject> scenes;

    [Header("Change Background")]
    public Image PanelImage_holder;
    public List<Sprite> newBackground;

    [Header("Filled")]
    public Image total_filled;

    [Header("Assessment 1")]
    public List<Button> rotatingButtons;
    public AudioSource assessment1_Instruction;
    public GameObject confetti;
    public float correctPuzzle = 0;
    public float fixedPuzzle = 0;
    
    [Header("Assessment 2")]
    public List<Button> button_choices;
    public Button correctNumber;

    [Header("Assessment 3")]
    public List<Button> crayon_choices;
    public Button correctNumberofCrayons;

    public float clickedWrong = 0;

    [Header("Assessment 5")]
    public List<Button> TsoundButtons;
    public Button correctTsoundButton;

    [Header("Achievement board")]
    public List<GameObject> Star_Display;
    public List<GameObject> Confetti_Sizes;
    public TextMeshProUGUI Complimentary_text;
    public GameObject original_complimentBoard;
    public GameObject originalStar_background;
    public GameObject zeroStar_background;
    public GameObject zeroStar_complimentBoard;

    [Header("Next Button")]
    public Button nextScene_Button;

    void Start()
    {
        background_music1.Play();
    }

    void Update()
    {
        if (scene_counter == 4)
        {
            if (background_music1.isPlaying)
            {
                background_music1.Stop();
            }
            if (!background_music2.isPlaying)
            {
                background_music2.Play();
            }
        }
    }

    public void whenButtonClicked()
    {
        if (scene_counter < 13)
        {
            if (scene_counter == 3 || scene_counter == 6 || scene_counter == 8 || scene_counter == 10)
            {
                UpdateScene();
                nextScene_Button.gameObject.SetActive(false);
                PanelImage_holder.sprite = newBackground[new_backgroundCounter];
                new_backgroundCounter++;
            }

            else if (scene_counter == 11)
            {
                UpdateScene();
                nextScene_Button.gameObject.SetActive(false);
                clickedWrong = 0;

                foreach (Button button in TsoundButtons)
                {
                    Button TButton = button;
                    TButton.onClick.AddListener(() => CheckAnswer(TButton, correctTsoundButton));
                }
            }

            else if (scene_counter == 12)
            {
                if (background_music2.isPlaying)
                {
                    background_music2.Stop();
                }

                UpdateScene();
                scenes[5].SetActive(false);
                Show_Stars();
            }

            else
            {
                UpdateScene();
                nextScene_Button.gameObject.SetActive(false);
            }
        }
    }

    public void OnAnimationEnd()
    {
        if (scene_counter == 4)
        {
            UpdateScene();
            scene_counter++;
            nextScene_Button.gameObject.SetActive(false);
            scenes[scene_counter].SetActive(true);
            PanelImage_holder.sprite = newBackground[new_backgroundCounter];
            new_backgroundCounter++;
            UpdateButtonState();
        }

        if (scene_counter == 7)
        {
            UpdateScene();
            PanelImage_holder.sprite = newBackground[new_backgroundCounter];
            new_backgroundCounter++;
            nextScene_Button.gameObject.SetActive(false);
            clickedWrong = 0;

            foreach (Button button in button_choices)
            {
                Button numberButton = button;
                numberButton.onClick.AddListener(() => CheckAnswer(numberButton, correctNumber));
            }

        }

        if (scene_counter == 9)
        {
            UpdateScene();
            PanelImage_holder.sprite = newBackground[new_backgroundCounter];
            new_backgroundCounter++;
            nextScene_Button.gameObject.SetActive(false);
            clickedWrong = 0;

            foreach (Button button in crayon_choices)
            {
                Button CrayonnumberButton = button;
                CrayonnumberButton.onClick.AddListener(() => CheckAnswer(CrayonnumberButton, correctNumberofCrayons));
            }
        }

        else
        {
            nextScene_Button.gameObject.SetActive(true);
        }
    }
    private void UpdateScene()
    {
        scenes[scene_counter].SetActive(false);
        scene_counter++;
        scenes[scene_counter].SetActive(true);
    }

    public void UpdateButtonState()
    {
        nextScene_Button.gameObject.SetActive(false);
        assessment1_Instruction.Play();

        if (assessment1_Instruction.isPlaying)
        {
            StartCoroutine(ReEnableButtonsAfterAudio());
        }
    }

    IEnumerator ReEnableButtonsAfterAudio()
    {
        foreach (Button button in rotatingButtons)
        {
            button.interactable = false;
        }

        while (assessment1_Instruction.isPlaying)
        {
            yield return null;
        }

        foreach (Button button in rotatingButtons)
        {
            button.interactable = true; ;
        }
    }

    public void SetFillAmount(float amount)
    {
        total_filled.fillAmount = Mathf.Clamp01(amount);
    }

    public void IncrementFillAmount(float amount)
    {
        total_filled.fillAmount = Mathf.Clamp01(total_filled.fillAmount + amount);
    }

    private void CheckAnswer(Button clicked, Button correctAnswer)
    {
        Debug.Log("Button clicked: " + clicked.name);

        if (clicked == correctAnswer)
        {
            if (clickedWrong == 0)
            {
                nextScene_Button.gameObject.SetActive(true);
                if (total_filled.fillAmount < 0.48f)
                {
                    IncrementFillAmount(0.16f);
                }

                else if (Mathf.Approximately(total_filled.fillAmount, 0.48f))
                {
                    SetFillAmount(0.598f);
                }

                else if (Mathf.Approximately(total_filled.fillAmount, 0.598f))
                {
                    SetFillAmount(0.72f);
                }

                else if (Mathf.Approximately(total_filled.fillAmount, 0.72f))
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
            clickedWrong++;
        }
    }

    void Show_Stars()
    {
        if (total_filled.fillAmount < 0.48f)
        {
            Star_Display[0].SetActive(true);
            Confetti_Sizes[0].SetActive(false);
            Confetti_Sizes[1].SetActive(false);
            zeroStar_background.SetActive(true);
            zeroStar_complimentBoard.SetActive(true);
            original_complimentBoard.SetActive(false);
            originalStar_background.SetActive(false);
            Complimentary_text.text = "ULITIN!";
        }
        else if (Mathf.Approximately(total_filled.fillAmount, 0.48f))
        {
            Star_Display[1].SetActive(true);
            Confetti_Sizes[1].SetActive(false);
            Complimentary_text.text = "SUBOK";
        }
        else if (Mathf.Approximately(total_filled.fillAmount, 0.72f))
        {
            Star_Display[2].SetActive(true);
            Complimentary_text.text = "MAGALING";
        }
        else if (Mathf.Approximately(total_filled.fillAmount, 1f))
        {
            Star_Display[3].SetActive(true);
            Complimentary_text.text = "PERPEKTO";
        }
    }
}

