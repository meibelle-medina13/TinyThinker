using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Collections;

public class Quarter1_Level3 : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private static int counter = 0;
    private static bool bgMusicPlayed = false;
    
    //DRAG
    private static int placedObjects = 0;
    private static int placed_wrong = 0;
    private Collider2D correctAnswerCollider;
    private Vector3 originalPosition;
    
    //ROTATE
    private float rotationAngle = 90f;
    private int angleCounter = 0;
    private static int correctPuzzle = 0;
    private static int fixedPuzzle = 0;
    
    //BUTTON
    private int wrong_click = 0;
    private static int instruction_count = 1;
    
    private Audio_Manager audioManager;
    
    public Image total_stars;
    public GameObject correctAnswerArea;
    public GameObject CorrectImage;
    public Button rotateButton;
    public int CorrectAngle = 2;
    public Button nextScene_Button;
    
    public List<GameObject> scenes;
    public List<TextMeshProUGUI> text;
    public List<Button> clickableButtons;
    public List<GameObject> Image;
    public List<GameObject> star_display;

    void Start()
    {
        originalPosition = transform.position;
        audioManager = FindObjectOfType<Audio_Manager>();

        if (!bgMusicPlayed)
        {
            if (audioManager != null)
            {
                audioManager.scene_bgmusic(); 
                bgMusicPlayed = true; 
            } 
        } 

        if (text[0] != null)
        {
            text[0].fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.3f);
            text[0].fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        }

        if (correctAnswerArea != null)
        {
            correctAnswerCollider = correctAnswerArea.GetComponent<Collider2D>();
        }

        if (rotateButton != null && CorrectImage != null)
        {
            rotateButton.onClick.AddListener(Rotate);
        }
    }

    void Update()
    {
        GameObject assessment = GameObject.Find("Assessment");

        if (counter == 3|| counter == 9)
        {
            nextScene_Button.gameObject.SetActive(false);
        }

        else if (counter == 11)
        {
            if (assessment != null && assessment.activeInHierarchy)
            {   
                assessment.SetActive(false);            
            }

            CancelInvoke("UpdateScene");
            Show_Stars();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CompareTag("Draggable")) 
        { transform.position = Input.mousePosition; }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CompareTag("Draggable")) return;

        bool isCorrect = correctAnswerCollider.bounds.Contains(transform.position);
        
        if (isCorrect)
        {
            audioManager.Correct();

            placedObjects++;
            UpdateNextButtonState();
            gameObject.SetActive(false);

            if (placed_wrong >= 1)
            {
                placed_wrong--;
                IncrementFillAmount(0.037037037037037f);
            }

            else
            {
               IncrementFillAmount(0.1111111111111111f);
            }
        }
        else
        { 
            audioManager.Wrong();
            transform.position = originalPosition;
            gameObject.SetActive(true);
            placed_wrong++ ;
        }
    }

    public void IncrementFillAmount(float amount)
    {
        total_stars.fillAmount = Mathf.Clamp01(total_stars.fillAmount + amount);
    }

    public void UpdateNextButtonState()
    {
        if (placedObjects == 3)
        {
            UpdateScene();
        }
    }

    public void UpdateScene()
    {
        scenes[counter].SetActive(false);
        counter++;
        scenes[counter].SetActive(true);

       if (counter == 6)
        {
            Image[0].SetActive(false);
            CancelInvoke("UpdateScene");
            nextScene_Button.gameObject.SetActive(true);
        }

        else if (counter == 7)
        {
            counter++;
            scenes[counter].SetActive(true);
            nextScene_Button.gameObject.SetActive(false);
            audioManager.assessment_bgmusic();
            audioManager.Repeat_Instruction(instruction_count);
            instruction_count++;
        }

        else if (counter == 9 || counter == 10)
        {
            wrong_click = 0;
            audioManager.Repeat_Instruction(instruction_count);
            instruction_count++;
        }

        else if (counter == 11)
        {
            audioManager.Stop_backgroundMusic2();
        }
    }

    public void SetFillAmount(float amount)
    {
        total_stars.fillAmount = Mathf.Clamp01(amount);
    }

    public void Rotate()
    {
        transform.Rotate(0f, 0f, rotationAngle);
        angleCounter++;

        if (angleCounter == CorrectAngle)
        {
            audioManager.Correct();
            gameObject.SetActive(false);
            CorrectImage.SetActive(true);
            correctPuzzle++;
            correctAnswer_Checker();
        }
        else
        {
            audioManager.Rotate();
        }
    }

    public void correctAnswer_Checker()
    {
        if (correctPuzzle == 4)
        {
            correctPuzzle = 0;
            fixedPuzzle++;
            Invoke("UpdateScene", 1);
        }

        if (fixedPuzzle == 2)
        {
            Image[0].SetActive(true);
            CancelInvoke("UpdateScene");
            Invoke("UpdateScene", 6);
        }  
    }

    public void CorrectButton(Button clickedButton)
    {
        audioManager.Correct();

        if (clickedButton.name == "Button_E")
        {
            Image[1].gameObject.SetActive(true);
            Invoke("UpdateScene", 2);
        }
        else
        {
            Invoke("UpdateScene", 2);
        }

        if (wrong_click >= 1)
        {
            IncrementFillAmount(0.037037037037037f);
        }

        else
        {
            IncrementFillAmount(0.3333333333333333f);
        }
    }

    public void WrongButton()
    {
        wrong_click++;
    }


    void Show_Stars()
    {
        nextScene_Button.gameObject.SetActive(false);

        if (total_stars.fillAmount < 0.3333333333333333f)
        {
            star_display[0].SetActive(true);
            Image[2].SetActive(false);
            Image[3].SetActive(false);
            Image[4].SetActive(true);
            Image[5].SetActive(true);
            Image[6].SetActive(false);
            Image[7].SetActive(false);
            text[1].text = "ULITIN!";
        }

        else if (total_stars.fillAmount >= 0.3333333333333333f && total_stars.fillAmount < 0.6666666666666667f)
        {
            star_display[1].SetActive(true);
            Image[2].SetActive(false);
            text[1].text = "SUBOK";
        }

        else if (total_stars.fillAmount >= 0.6666666666666667f && total_stars.fillAmount < 1f)
        {
            star_display[2].SetActive(true);
            text[1].text = "MAGALING";
        }

        else if (Mathf.Approximately(total_stars.fillAmount, 1f))
        {
            star_display[3].SetActive(true);
            text[1].text = "PERPEKTO";
        }
    }
}


