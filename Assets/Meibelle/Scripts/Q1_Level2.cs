using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Q1_Level2 : MonoBehaviour
{
    [Header("<---- SCENE PANELS ---->")]
    [SerializeField]
    private GameObject[] scenes = new GameObject[11];
    [SerializeField]
    private GameObject[] assessments = new GameObject[4];

    [Header("<---- TRACING PANELS ---->")]
    [SerializeField]
    private GameObject instructions;
    [SerializeField]
    private GameObject tracingpanel;

    [Header("<---- TRACING NEXT BUTTON ---->")]
    [SerializeField]
    private Button tracing_button;

    [Header("<---- EXERCISE PANELS ---->")]
    [SerializeField]
    private Button[] exercise1 = new Button[8];
    [SerializeField]
    private Button[] exercise2 = new Button[2];

    [Header("<---- ASSESSMENT GAMEOBJECTS ---->")]
    [SerializeField]
    private Button treasureBox;
    [SerializeField]
    private GameObject tracingObject;
    [SerializeField]
    private GameObject[] assessment2 = new GameObject[4];
    Vector3[] keyInitialPos = new Vector3[3];
    [SerializeField]
    private GameObject[] assessment3 = new GameObject[2];
    [SerializeField]
    private GameObject[] ballons = new GameObject[2];
    [SerializeField]
    private GameObject sticker;

    [Header("<---- PROGRESS BAR AND RESULT ---->")]
    [SerializeField]
    private Image progressBar;
    [SerializeField]
    private GameObject[] result = new GameObject[5];

    [Header("<---- STARS IMAGE AND SPRITE ---->")]
    [SerializeField]
    private Image[] stars = new Image[3];
    [SerializeField]
    private Sprite earnedStar;

    [Header("<---- GAME MENU ---->")]
    [SerializeField]
    private GameObject gameMenu;

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private THEME1_LEVEL1_REQUESTS requestsManager;

    private int num_of_tracing_points = 39;
    private int assess1 = 100;
    private int assess2 = 100;
    private int assess3 = 100;
    private int error;
    private float score;
    private int userID;

    private void Start()
    {
        requestsManager = FindObjectOfType<THEME1_LEVEL1_REQUESTS>();

        PlayerPrefs.SetInt("Tracing Points", 0);
        userID = PlayerPrefs.GetInt("Current_user");

        for (int i = 0; i < (scenes.Length - 1); i++)
        {
            int index = i;
            Button nextButton = scenes[i].GetComponentInChildren<Button>();
            if (nextButton != null && nextButton.name == "next-button")
            {
                nextButton.onClick.AddListener(() => OnContinue(index));
            }
        }

        foreach (Button letter in exercise1)
        {
            letter.onClick.AddListener(() => CheckExercise1(letter));
        }

        foreach (Button number in exercise2)
        {
            number.onClick.AddListener(() => CheckExercise2(number));
        }

        treasureBox.onClick.AddListener(() => OpenAssessment1());

        for (int i = 0; i < (assessment2.Length - 1); i++)
        {
            keyInitialPos[i] = assessment2[i].transform.position;
        }

        tracing_button.onClick.AddListener(() => CheckAssessment1());

        foreach (GameObject balloon in ballons)
        {
            Button button = balloon.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => CheckAssessment3(balloon));
        }
    }

    private void Update()
    {
        PlayableDirector playableDirector;

        int index = PlayerPrefs.GetInt("CurrentPanel");
        Debug.Log(index);
        if (!assessments[3].activeSelf)
        {
            if (!scenes[10].activeSelf)
            {
                Debug.Log(scenes[index].name);
                playableDirector = scenes[index].GetComponent<PlayableDirector>();
                //if (scenes[index].name == "Scene5")
                //{
                //    GameObject scene = scenes[index].transform.Find("Scene 5.1").gameObject;
                //    playableDirector = scene.GetComponent<PlayableDirector>();
                //}
                //else
                //{
                //    playableDirector = scenes[index].GetComponent<PlayableDirector>();
                //}
            }
            else
            {
                playableDirector = assessments[index].GetComponent<PlayableDirector>();
            }

            if (PlayerPrefs.GetString("Paused") == "True")
            {
                playableDirector.Pause();
                if (tracingpanel.activeSelf)
                {
                    tracingObject.SetActive(false);
                }
            }
            else if (PlayerPrefs.GetString("Paused") == "False")
            {
                playableDirector.Resume();
                if (tracingpanel.activeSelf)
                {
                    tracingObject.SetActive(true);
                }
            }
        }
        else
        {
            gameMenu.SetActive(false);
        }
    }
    public void OpenPreview()
    {
        scenes[0].SetActive(true);
        PlayerPrefs.SetInt("CurrentPanel", 0);
    }

    public void OnContinue(int index)
    {
        scenes[index].SetActive(false);
        scenes[index + 1].SetActive(true);
        if (index == 9)
        {
            PlayerPrefs.SetInt("CurrentPanel", 0);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentPanel", index + 1);
        }
    }

    private void CheckExercise1(Button letter)
    {
        string text = letter.name;
        if (text != "next-button")
        {
            int correct = 0;
            ColorBlock letterColor = letter.colors;

            if (text == "Aa")
            {
                letterColor.normalColor = Color.green;
                letterColor.selectedColor = Color.green;
            }
            else
            {
                letterColor.selectedColor = Color.black;
            }
            letter.colors = letterColor;

            for (int i = 0; i < 7; i++)
            {
                ColorBlock bc = exercise1[i].colors;
                if (bc.normalColor == Color.green)
                {
                    correct++;
                }
            }

            if (correct == 3)
            {
                exercise1[7].interactable = true;   
                exercise1[7].image.color = new Color(255f, 255f, 255f, 255f);
                exercise1[7].onClick.AddListener(() => OnContinue(5));
            }
        }
    }

    private void CheckExercise2(Button number)
    {
        string text = number.name;
        if (text == "1")
        {
            OnContinue(8);
        }
    }

    private void OpenAssessment1()
    {
        instructions.SetActive(false);
        tracingpanel.SetActive(true);
    }

    private void CheckAssessment1()
    {
        assessments[0].SetActive(false);
        assessments[1].SetActive(true);
        PlayerPrefs.SetInt("CurrentPanel", 1);
        MoveProgress(error, 1);
    }

    public void DragKey(GameObject key)
    {
        key.transform.position = Input.mousePosition;
    }

    public void DropKey(GameObject key)
    {
        float Distance = Vector3.Distance(key.transform.position, assessment2[3].transform.position);
        
        if (key.name == "key-a" && Distance < 150)
        {
            key.transform.Rotate(0, 0, -96);
            key.transform.position = new Vector3(850, 197, 0);
            MoveProgress(error, 2);
            StartCoroutine(DelayAssessment3());
        }
        else if (key.name == "key-a")
        {
            error += 50;
            key.transform.position = keyInitialPos[0];
        }
        else if (key.name == "key-e")
        {
            error += 50;
            key.transform.position = keyInitialPos[1];
        }
        else if (key.name == "key-h")
        {
            error += 50;
            key.transform.position = keyInitialPos[2];
        }
    }

    IEnumerator DelayAssessment3()
    {
        yield return new WaitForSeconds(1.5f);
        assessments[1].SetActive(false);
        assessments[2].SetActive(true);
        PlayerPrefs.SetInt("CurrentPanel", 2);
    }

    private void CheckAssessment3(GameObject balloon)
    {
        if (balloon.name == "1")
        {
            balloon.SetActive(false);
            sticker.SetActive(true);
            MoveProgress(error, 3);
            StartCoroutine(DelayResult());
        }
        else
        {
            error += 100;
        }
    }

    IEnumerator DelayResult()
    {
        yield return new WaitForSeconds(1.5f);
        assessment3[0].SetActive(false);
        assessment3[1].SetActive(true);
    }

    public void GetResult()
    {
        assessments[2].SetActive(false);
        assessments[3].SetActive(true);
        AssessResult();
    }

    private void MoveProgress(int totalError, int assessNum)
    {
        float currentScore = 0;
        if (assessNum == 1)
        {
            int points = PlayerPrefs.GetInt("Tracing Points");
            float score_per_point = assess1 / (float)num_of_tracing_points;
            float a1 = ((float)(score_per_point * points) / assess1) * (100f / 3f);
            currentScore = a1;
        }
        else if (assessNum == 2)
        {
            float a2 = ((float)(assess2 - totalError) / assess2) * (100f / 3f);
            if (a2 <= 0)
            {
                currentScore = 0;
            }
            else
            {
                currentScore = a2;
            }
        }
        else if (assessNum == 3)
        {
            float a3 = ((float)(assess3 - totalError) / assess3) * (100f / 3f);
            if (a3 <= 0)
            {
                currentScore = 0;
            }
            else
            {
                currentScore = a3;
            }
        }
        error = 0;

        score += currentScore;
        progressBar.fillAmount = score / 100;
        if (score >= (100f / 3f) * 1 && score < (100f / 3f) * 2)
        {
            stars[0].sprite = earnedStar;
        }
        else if (score >= (100f / 3f) * 2 && score < (100f / 3f) * 3)
        {
            stars[0].sprite = earnedStar;
            stars[1].sprite = earnedStar;
        }
        else if (score == (100f / 3f) * 3)
        {
            stars[0].sprite = earnedStar;
            stars[1].sprite = earnedStar;
            stars[2].sprite = earnedStar;
        }
    }

    private int delaytime;

    private void AssessResult()
    {
        int theme_num = 1;
        int level_num = 2;
        if (PlayerPrefs.GetFloat(userID.ToString() + "Time") > 0)
        {
            StartCoroutine(requestsManager.UpdateCurrentScore("/scores", score, userID, theme_num, level_num));
        }

        float star1 = (100f / 3f);
        float star2 = (100f / 3f) * 2;
        float star3 = (100f / 3f) * 3;

        if (score < star1)
        {
            result[4].SetActive(true);
            delaytime = 8;
        }
        else if (score >= star1 && score < star2)
        {
            result[1].SetActive(true);
            delaytime = 12;
        }
        else if (score >= star2 && score < star3)
        {
            delaytime = 12;
            result[0].SetActive(true);
            result[2].SetActive(true);
        }
        else
        {
            delaytime += 20;
            result[0].SetActive(true);
            result[3].SetActive(true);
        }
        StartCoroutine(GoToMap());

        if (score > (100f / 3f))
        {
            StartCoroutine(requestsManager.AddReward("/reward", userID, 2));
        }
    }

    IEnumerator GoToMap()
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
                int next_level = 3;
                StartCoroutine(requestsManager.UpdateCurrentLevel("/users/updateLevel", next_level, userID));
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(7);
            }
        }
    }
}


