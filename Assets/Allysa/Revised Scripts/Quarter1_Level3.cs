using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Playables;

public class Quarter1_Level3 : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private static int counter = 0;
    //private static bool bgMusicPlayed = false;
    
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
    public List<GameObject> timelines;
    public List<TextMeshProUGUI> text;
    public List<Button> clickableButtons;
    public List<GameObject> Image;
    public List<GameObject> star_display;

    [Header("<---- GAME MENU ---->")]
    [SerializeField]
    private GameObject gameMenu;


    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private THEME1_LEVEL1_REQUESTS requestsManager;

    void Start()
    {
        if (gameObject.name == "Level3 Scene Manager")
        {
            counter = 0;
        }
        Debug.Log(counter);
        requestsManager = FindObjectOfType<THEME1_LEVEL1_REQUESTS>();

        originalPosition = transform.position;
        audioManager = FindObjectOfType<Audio_Manager>();

        //if (!bgMusicPlayed)
        //{
        //    if (audioManager != null)
        //    {
        //        audioManager.scene_bgmusic(0.5f); 
        //        bgMusicPlayed = true; 
        //    } 
        //} 

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

        if (counter == 3|| counter == 9)
        {
            nextScene_Button.gameObject.SetActive(false);
        }

        else if (counter == 11)
        {
            GameObject assessment = GameObject.Find("Assessment");

            if (assessment != null && assessment.activeInHierarchy)
            {   
                assessment.SetActive(false);            
            }

            CancelInvoke("UpdateScene");
            //Show_Stars();
        }

        if (total_stars.fillAmount >= 0.3333333333333333f && total_stars.fillAmount < 0.6666666666666667f)
        {
            Image[7].SetActive(false);
        }

        else if (total_stars.fillAmount >= 0.6666666666666667f && total_stars.fillAmount < 1f)
        {
            Image[8].SetActive(false);
        }

        else if (Mathf.Approximately(total_stars.fillAmount, 1f))
        {
            Image[9].SetActive(false);
        }

        int index = 0;

        if (gameObject.name == "Level3 Scene Manager")
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                if (scenes[i].activeSelf)
                {
                    index = i;
                    if (index != 7)
                    {
                        Debug.Log(index);
                        break;
                    }
                }
            }

            if (index != 11)
            {
                PlayableDirector playableDirector;

                if (scenes[index].name == "Scene 7")
                {
                    index -= 2;
                }
                else if (index >= 8)
                {
                    index -= 3;
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

        PlayerPrefs.SetInt("CurrentPanel", counter);

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
            //audioManager.assessment_bgmusic(0.5f);
            //audioManager.Repeat_Instruction(instruction_count);
            instruction_count++;
        }

        else if (counter == 9 || counter == 10)
        {
            wrong_click = 0;
            //audioManager.Repeat_Instruction(instruction_count);
            instruction_count++;
        }

        else if (counter == 11)
        {
            //audioManager.Stop_backgroundMusic2();
            // ------------------------------------------------------------------- //
            Show_Stars();
        }
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
        float score = total_stars.fillAmount * 100;
        int userID = PlayerPrefs.GetInt("Current_user");
        int theme_num = 1;
        int level_num = 3;
        int delaytime = 0;

        if (PlayerPrefs.GetFloat("Time") > 0)
        {
            StartCoroutine(requestsManager.UpdateCurrentScore("/scores", score, userID, theme_num, level_num));
        }

        if (total_stars.fillAmount < 0.3333333333333333f)
        {
            star_display[0].SetActive(true);
            Image[2].SetActive(false);
            Image[3].SetActive(false);
            Image[4].SetActive(true);
            Image[5].SetActive(true);
            Image[6].SetActive(false);
            Image[7].SetActive(false);
            delaytime = 8;
        }

        else if (total_stars.fillAmount >= 0.3333333333333333f && total_stars.fillAmount < 0.6666666666666667f)
        {
            star_display[1].SetActive(true);
            Image[2].SetActive(false);
            delaytime = 10;
        }

        else if (total_stars.fillAmount >= 0.6666666666666667f && total_stars.fillAmount < 1f)
        {
            star_display[2].SetActive(true);
            delaytime = 10;
        }

        else if (Mathf.Approximately(total_stars.fillAmount, 1f))
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
            if (PlayerPrefs.GetFloat("Time") > 0)
            {
                int next_level = 4;
                StartCoroutine(requestsManager.UpdateCurrentLevel("/users/updateLevel", next_level, userID));
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(7);
            }
        }
    }
}