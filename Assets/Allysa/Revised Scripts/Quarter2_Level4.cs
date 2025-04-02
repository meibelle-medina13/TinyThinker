using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using System.Linq;

public class Quarter2_Level4 : MonoBehaviour
{
    private static int Scene_counter = 0;
    //private static bool bgMusicPlayed = false;
    private Vector3 pencilWrite = new Vector3(85, 100, 0);
    private Vector3 pencilState;
    private Vector3 pencilRaise = new Vector3(105, 120, 0);
    private HashSet<string> tracedPoints = new HashSet<string>();
    private int score = 0;
    private static int frame_2;
    private static int frame_3;
    //private static int instruction_count = 1;
    private Audio_Manager audioManager3;

    //BUTTON
    private int Wrong_Click = 0;

    //ROTATE
    private float rotationAngle = 90f;
    private int angleCounter = 0;
    //private static int correctPuzzle = 0;
    private static int fixedPuzzle = 0;

    public List<GameObject> scenes;
    public List<GameObject> timelines;
    public List<GameObject> Gameobjects;
    public List<TextMeshProUGUI> text;
    public List<Button> clickablebuttons;
    public List<GameObject> star_display;

    public GameObject CorrectImage;
    public Button rotateButton;
    public Image fill_bar;
    public int CorrectAngle = 2;
    public int totalTracingPoints = 0;

    public Button NextScene_Button;

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private THEME1_LEVEL1_REQUESTS requestsManager;

    [Header("<---- GAME MENU ---->")]
    [SerializeField]
    private GameObject gameMenu;

    [Header("<---- TRACING OBJECTS ---->")]
    [SerializeField]
    private GameObject[] tracingObjects = new GameObject[3];

    void Start()
    {
        if (gameObject.name == "Theme 2- Level 4 Scene Manager")
        {
            Scene_counter = 0;
        }

        requestsManager = FindObjectOfType<THEME1_LEVEL1_REQUESTS>();

        audioManager3 = FindObjectOfType<Audio_Manager>();

        if (Gameobjects[0] != null)
        {
            if (Gameobjects[0].name == "Scene2" || Gameobjects[0].name == "Scene4" || Gameobjects[0].name == "Scene6")
            {
                NextScene_Button.gameObject.SetActive(false);
            }
            else
            {
                NextScene_Button.gameObject.SetActive(true);
            }
        }


        //if (!bgMusicPlayed)
        //{
        //    if (audioManager3 != null)
        //    {
        //        audioManager3.scene_bgmusic(0.26f);
        //        bgMusicPlayed = true;
        //    }
        //}

        if (text[0] != null)
        {
            text[0].fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.3f);
            text[0].fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        }

        if (rotateButton != null && CorrectImage != null)
        {
            rotateButton.onClick.AddListener(Rotate);
        }
    }

    void Update()
    {
        if (Scene_counter == 0 || Scene_counter == 2 || Scene_counter == 4)
        {
            NextScene_Button.gameObject.SetActive(false);
        }

        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane + 1;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (Gameobjects[0] != null && Gameobjects[1] != null && Gameobjects[2] != null && Gameobjects[3] != null)
        {
            if (Input.GetMouseButton(0))
            {
                Gameobjects[3].GetComponent<CircleCollider2D>().enabled = true;
                pencilState = pencilWrite;

                GameObject pencilMask = Instantiate(Gameobjects[2], worldPosition, Quaternion.identity);
                pencilMask.transform.SetParent(Gameobjects[0].transform);
            }

            else
            {
                Gameobjects[3].GetComponent<CircleCollider2D>().enabled = false;
                pencilState = pencilRaise;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Gameobjects[3].GetComponent<CircleCollider2D>().enabled = false;
                pencilState = pencilRaise;
            }

            Gameobjects[1].transform.position = worldPosition + pencilState;
            Gameobjects[3].transform.position = worldPosition;
        }

        int index = 0;

        int[] noTimelines = { 1, 3, 5, 11, 13 };

        if (gameObject.name == "Theme 2- Level 4 Scene Manager")
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                if (scenes[i].activeSelf)
                {
                    index = i;
                    if (index != 8)
                    {
                        break;
                    }
                }
            }

            Debug.Log("Panel" + index);

            if (index != 16 && !noTimelines.Contains(index))
            {
                PlayableDirector playableDirector;
                if (index == 2)
                {
                    index -= 1;
                }
                else if (index == 4)
                {
                    index -= 2;
                }
                else if (index == 6 || index == 7)
                {
                    index -= 3;
                }
                else if (index == 8)
                {
                    index = 9;
                }

                Debug.Log("Panel" + index);
                if (scenes[index].name == "Assessment 1")
                {
                    index = 9;
                }
                else if (scenes[index].name == "zoom out assessment 1")
                {
                    index = 5;
                }
                else if (scenes[index].name == "zoom out assessment 2")
                {
                    index = 6;
                }
                else if (scenes[index].name == "Assessment 4")
                {
                    index = 7;
                }
                else if (scenes[index].name == "Assessment 5")
                {
                    index = 8;
                }

                playableDirector = timelines[index].GetComponent<PlayableDirector>();

                if (PlayerPrefs.GetString("Paused") == "True")
                {
                    playableDirector.Pause();
                    if (scenes[14].activeSelf)
                    {
                        tracingObjects[3].SetActive(false);
                    }
                }
                else
                {
                    playableDirector.Resume();
                    if (scenes[14].activeSelf)
                    {
                        tracingObjects[3].SetActive(true);
                    }
                }
            }
            else
            {
                if (index == 1)
                {
                    if (PlayerPrefs.GetString("Paused") == "True")
                    {
                        tracingObjects[0].SetActive(false);
                    }
                    else
                    {
                        tracingObjects[0].SetActive(true);
                    }
                }
                else if (index == 3)
                {
                    if (PlayerPrefs.GetString("Paused") == "True")
                    {
                        tracingObjects[1].SetActive(false);
                    }
                    else
                    {
                        tracingObjects[1].SetActive(true);
                    }
                }
                else if (index == 5)
                {
                    if (PlayerPrefs.GetString("Paused") == "True")
                    {
                        tracingObjects[2].SetActive(false);
                    }
                    else
                    {
                        tracingObjects[2].SetActive(true);
                    }
                }
                else if (index == 16)
                {
                    gameMenu.SetActive(false);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tracing Point") && !tracedPoints.Contains(other.gameObject.name))
        {
            tracedPoints.Add(other.gameObject.name);
            score++;
            CheckCompletion();
        }
    }

    void CheckCompletion()
    {
        if (Gameobjects[0].name == "Scene2" || Gameobjects[0].name == "Scene4" || Gameobjects[0].name == "Scene6")
        {
            if (tracedPoints.Count >= totalTracingPoints)
            {
                audioManager3.Correct();
                NextScene_Button.gameObject.SetActive(true);
            }
        }
    }

    public void UpdateGrade()
    {
        float percentage = (float)score / totalTracingPoints * 100;
        string grade = GetGrade(percentage);
        Debug.Log("Grade: " + grade);

        if (grade == "A")
        {
            IncrementFillAmount(0.2f);
        }
        else if (grade == "B")
        {
            IncrementFillAmount(0.15f);
        }
        else if (grade == "C")
        {
            IncrementFillAmount(0.1f);
        }
        else if (grade == "D")
        {
            IncrementFillAmount(0.05f);
        }

        progressFillChecker();
        UpdateScene();
    }

    string GetGrade(float percentage)
    {
        if (percentage >= 90) return "A";
        if (percentage >= 80) return "B";
        if (percentage >= 70) return "C";
        if (percentage >= 60) return "D";
        return "F";
    }

    //////


    public void UpdateScene()
    {
        scenes[Scene_counter].SetActive(false);
        Scene_counter++;
        scenes[Scene_counter].SetActive(true);

        if (Scene_counter == 7)
        {
            //audioManager3.assessment_bgmusic(0.5f);
            NextScene_Button.gameObject.SetActive(false);
        }

        else if (Scene_counter == 8)
        {
            Scene_counter++;
            scenes[Scene_counter].SetActive(true);
            //audioManager3.Repeat_Instruction(0);
        }

        else if (Scene_counter == 10)
        {
            CancelInvoke("UpdateScene");
        }

        else if (Scene_counter == 15)
        {
            NextScene_Button.gameObject.SetActive(false);
            //audioManager3.Repeat_Instruction(4);
        }

        else if (Scene_counter == 16)
        {
            //audioManager3.Stop_backgroundMusic2();
            Show_Stars();
        }
    }

    public void IncrementFillAmount(float amount)
    {
        fill_bar.fillAmount = Mathf.Clamp01(fill_bar.fillAmount + amount);
    }

    public void Rotate()
    {
        transform.Rotate(0f, 0f, rotationAngle);
        angleCounter++;

        if (angleCounter == CorrectAngle)
        {
            audioManager3.Correct();
            gameObject.SetActive(false);
            CorrectImage.SetActive(true);

            if (rotateButton.name == "1 of 4 in 2" ||
                rotateButton.name == "2 of 4 in 2" ||
                rotateButton.name == "3 of 4 in 2" ||
                rotateButton.name == "4 of 4 in 2")
            {
                frame_2++;
                Debug.Log("2: " + frame_2);
            }

            if (rotateButton.name == "1 of 4 in 3" ||
                rotateButton.name == "2 of 4 in 3" ||
                rotateButton.name == "3 of 4 in 3" ||
                rotateButton.name == "4 of 4 in 3")
            {
                frame_3++;
                Debug.Log("3: " + frame_3);
            }

            correctAnswer_Checker();
        }
        else
        {
            audioManager3.Rotate();
        }
    }

    public void correctAnswer_Checker()
    {
        if (fixedPuzzle == 1)
        {
            if (frame_2 == 4 || frame_3 == 4)
            {
                Gameobjects[4].SetActive(true);
                IncrementFillAmount(0.1f);
                Invoke("UpdateScene", 6);
            }
        }

        else if (frame_2 == 4)
        {
            fixedPuzzle++;
            IncrementFillAmount(0.1f);
            frame_2 = 0;
        }

        else if (frame_3 == 4)
        {
            fixedPuzzle++;
            IncrementFillAmount(0.1f);
            frame_3 = 0;
        }

        progressFillChecker();
    }

    public void CorrectButton()
    {
        if (Wrong_Click >= 2)
        {
            IncrementFillAmount(0.0666666666666667f);
        }

        else if (Wrong_Click == 1)
        {
            IncrementFillAmount(0.1333333333333334f);
        }

        else
        {
            IncrementFillAmount(0.2f);
        }

        Wrong_Click = 0;
        progressFillChecker();
        Invoke("UpdateScene", 2);
    }

    public void WrongButton()
    {
        Wrong_Click++;
    }


    private void progressFillChecker()
    {
        if (fill_bar.fillAmount >= 0.3333333333333333f && fill_bar.fillAmount < 0.6666666666666667f)
        {
            if (Gameobjects.Count == 8)
            {
                Gameobjects[5].SetActive(false);
            }

            else if (Gameobjects.Count == 12)
            {
                Gameobjects[9].SetActive(false);
            }
        }

        else if (fill_bar.fillAmount >= 0.6666666666666667f && fill_bar.fillAmount < 1f)
        {
            if (Gameobjects.Count == 8)
            {
                Gameobjects[6].SetActive(false);
            }

            else if (Gameobjects.Count == 12)
            {
                Gameobjects[10].SetActive(false);
            }
        }

        else if (Mathf.Approximately(fill_bar.fillAmount, 1f))
        {
            if (Gameobjects.Count == 8)
            {
                Gameobjects[7].SetActive(false);
            }

            else if (Gameobjects.Count == 12)
            {
                Gameobjects[11].SetActive(false);
            }
        }
    }

    void Show_Stars()
    {
        NextScene_Button.gameObject.SetActive(false);

        float score = fill_bar.fillAmount * 100;
        int userID = PlayerPrefs.GetInt("Current_user");
        int theme_num = 2;
        int level_num = 4;
        int delaytime = 0;

        if (PlayerPrefs.GetFloat(userID.ToString() + "Time") > 0)
        {
            StartCoroutine(requestsManager.UpdateCurrentScore("/scores", score, userID, theme_num, level_num));
        }

        if (fill_bar.fillAmount < 0.3333333333333333f)
        {
            star_display[0].SetActive(true);
            Gameobjects[8].SetActive(false);
            Gameobjects[9].SetActive(false);
            Gameobjects[10].SetActive(false);
            Gameobjects[11].SetActive(false);
            delaytime = 8;
        }

        else if (fill_bar.fillAmount >= 0.3333333333333333f && fill_bar.fillAmount < 0.6666666666666667f)
        {
            star_display[1].SetActive(true);
            Gameobjects[8].SetActive(false);
            delaytime = 12;
        }

        else if (fill_bar.fillAmount >= 0.6666666666666667f && fill_bar.fillAmount < 1f)
        {
            star_display[2].SetActive(true);
            delaytime = 12;
        }

        else if (Mathf.Approximately(fill_bar.fillAmount, 1f))
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

