using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.Playables;
using UnityEngine.EventSystems;

public class Q1_Level1 : MonoBehaviour
{
    [Header("<---- SCENE PANELS ---->")]
    [SerializeField]
    private GameObject[] scenes = new GameObject[9];
    [SerializeField]
    private GameObject[] assessments = new GameObject[4];

    [Header("<---- EXERCISES GAMEOBJECTS ---->")]
    [SerializeField]
    private GameObject[] exercise1 = new GameObject[4];
    [SerializeField] 
    private GameObject[] exercise2 = new GameObject[7];
    Vector3[] candlesInitialPos = new Vector3[4];

    [Header("<---- ASSESSMENTS GAMEOBJECTS ---->")]
    [SerializeField]
    private Button[] assessment1Button = new Button[3];
    [SerializeField]
    private TextMeshProUGUI[] assessment1Text = new TextMeshProUGUI[3];
    [SerializeField]
    private Button[] assessment2Button = new Button[3];
    [SerializeField]
    private Button[] assessment3Button = new Button[3];
    [SerializeField]
    private GameObject[] assessmentConfetti = new GameObject[3];

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

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioClip;

    private int assess1 = 100;
    private int assess2 = 100;
    private int assess3 = 100;
    private int error;
    private int userID;
    private float score;
    int positionedCandles;

    public void Start()
    {
        PlayerPrefs.DeleteKey("CurrentPanel");
        requestsManager = FindObjectOfType<THEME1_LEVEL1_REQUESTS>();

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

        for (int i = 0; i < (exercise1.Length - 1); i++)
        {
            Button option = exercise1[i].GetComponentInChildren<Button>();
            if (option.name == "nametag")
            {
                option.onClick.AddListener(() => ShowConfetti(exercise1[3]));
            }
        }

        for (int i = 3; i < exercise2.Length; i++)
        {
            candlesInitialPos[i - 3] = exercise2[i].transform.position;
        }

        Assessment1ShuffledLetters(0);

        foreach (Button button in assessment2Button)
        {
            string text = button.name;
            button.onClick.AddListener(() => CheckAssessment2(text));
        }

        foreach (Button button in assessment3Button)
        {
            string text = button.name;
            button.onClick.AddListener(() => CheckAssessment3(text));
        }
    }

    private void Update()
    {
        PlayableDirector playableDirector;

        int index = PlayerPrefs.GetInt("CurrentPanel");
        Debug.Log(index);
        if (!assessments[3].activeSelf)
        {
            if (!scenes[8].activeSelf)
            {
                Debug.Log(scenes[index].name);
                if (scenes[index].name == "Scene5")
                {
                    GameObject scene = scenes[index].transform.Find("Scene 5.1").gameObject;
                    playableDirector = scene.GetComponent<PlayableDirector>();
                } 
                else
                {
                    playableDirector = scenes[index].GetComponent<PlayableDirector>();
                }
            }
            else
            {
                playableDirector = assessments[index].GetComponent<PlayableDirector>();
            }

            if (PlayerPrefs.GetString("Paused") == "True")
            {
                playableDirector.Pause();
            }
            else if (PlayerPrefs.GetString("Paused") == "False")
            {
                playableDirector.Resume();
            }
        }
    }

    public void OpenPreview()
    {
        scenes[0].SetActive(true);
        PlayerPrefs.SetInt("CurrentPanel", 0);
    }

    public void OnContinue(int index)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            selectedButton.enabled = false;
        }

        scenes[index].SetActive(false);
        scenes[index+1].SetActive(true);
        if (index == 7)
        {
            PlayerPrefs.SetInt("CurrentPanel", 0);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentPanel", index + 1);
        }
    }

    private void ShowConfetti(GameObject obj)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            selectedButton.enabled = false;
        }
        obj.SetActive(true);
    }

    public void DragCandle(GameObject candle)
    {
        candle.transform.position = Input.mousePosition;
    }

    public void DropCandle(GameObject candle)
    {
        GameObject cake = exercise2[2];
        float[] positions = new float[] { 460.97f, 507.46f, 551.96f, 604.71f };
        float Distance = Vector3.Distance(candle.transform.position, cake.transform.position);

        int index;
        string name = candle.name;

        for (int i = 1; i <= 4; i++)
        {
            if (name == "candle" + i.ToString())
            {
                index = i;
                if (Distance < 150)
                {
                    audioSource.PlayOneShot(audioClip);
                    candle.transform.position = new Vector3(positions[index - 1], 398.80f, 0.00f);
                    positionedCandles += 1;
                    
                    if (candle.TryGetComponent<EventTrigger>(out var trigger))
                    {
                        trigger.triggers.RemoveAll(e => e.eventID == EventTriggerType.Drag);
                    }
        

                    if (positionedCandles == 4)
                    {
                        exercise2[0].SetActive(false);
                        exercise2[1].SetActive(true);
                    }
                }
                else
                {
                    candle.transform.position = candlesInitialPos[index - 1];
                }
                break;
            }
        }
    }

    private void Assessment1ShuffledLetters(int index)
    {
        int current_index = UnityEngine.Random.Range(0, 3);
        string letter = assessment1Text[index].text;
        assessment1Button[current_index].GetComponentInChildren<TMP_Text>().text = letter;

        for (int j = 0; j < 3; j++)
        {
            if (j != current_index)
            {
                string randomLetter = ChooseRandomLetter();
                assessment1Button[j].GetComponentInChildren<TMP_Text>().text = randomLetter;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            string text = assessment1Button[i].GetComponentInChildren<TMP_Text>().text;
            assessment1Button[i].onClick.RemoveAllListeners();
            assessment1Button[i].onClick.AddListener(() => CheckAssessment1(index, text));
        }
    }

    private string ChooseRandomLetter()
    {
        string letters = "B,C,D,E,F,G,H,I,J,K,L,M,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        string[] letterArray = letters.Split(',');

        return letterArray[UnityEngine.Random.Range(0, 24)];
    }

    private void CheckAssessment1(int i, string word)
    {
        float prev = score;
        if (assessment1Text[i].color != Color.black && assessment1Text[i].text == word)
        {
            assessment1Text[i].color = Color.black;
            EventSystem.current.SetSelectedGameObject(null);
            if (i < 2)
            {
                Assessment1ShuffledLetters(i + 1);
                MoveProgress(error, 1);
            }
            else
            {
                MoveProgress(error, 1);
                for (int j = 0; j < assessment1Button.Length; j++)
                {
                    assessment1Button[j].enabled = false;
                }
                ShowConfetti(assessmentConfetti[0]);
            }
        }
        else
        {
            error += 16;
        }
    }

    private void CheckAssessment2(string word)
    {
        float prev = score;
        if (word == "nametag")
        {
            MoveProgress(error, 2);
            for (int j = 0; j < assessment2Button.Length; j++)
            {
                assessment2Button[j].enabled = false;
            }
            ShowConfetti(assessmentConfetti[1]);
        }
        else
        {
            error += 50;
        }
    }

    private void CheckAssessment3(string word)
    {
        float prev = score;
        if (word == "four")
        {
            MoveProgress(error, 3);
            for (int j = 0; j < assessment3Button.Length; j++)
            {
                assessment3Button[j].enabled = false;
            }
            ShowConfetti(assessmentConfetti[2]);
        }
        else
        {
            error += 50;
        }
    }

    public void NextAssessment(int index)
    {
        assessments[index].SetActive(false);
        assessments[index + 1].SetActive(true);
        PlayerPrefs.SetInt("CurrentPanel", index+1);
        if (index + 1 == 3)
        {
            AssessResult();
            gameMenu.SetActive(false);
        }
    }

    private void MoveProgress(int totalError, int assessNum)
    {
        float currentScore = 0;
        if (assessNum == 1)
        {
            float a1 = ((float)(((float)assess1/3) - totalError)/assess1) * (100f / 3f);
            if (a1 <= 0)
            {
                currentScore = 0;
            }
            else
            {
                currentScore = a1;
            }
        }
        else if (assessNum == 2) 
        {
            float a2 = ((float) (assess2 - totalError) / assess2) * (100f  / 3f);
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
        print("score" + score);
        progressBar.fillAmount = score / 100;
        if (score >= (100f / 3f) * 1 && score < (100f / 3f) * 2)
        {
            Debug.Log("Star1");
            stars[0].sprite = earnedStar;
        }
        else if (score >= (100f/3f) * 2 && score < (100f / 3f) * 3)
        {
            stars[0].sprite = earnedStar;
            stars[1].sprite = earnedStar;
        }
        else if (score == (100f / 3f) * 3) {
            stars[0].sprite = earnedStar;
            stars[1].sprite = earnedStar;
            stars[2].sprite = earnedStar;
        }
    }

    private int delaytime;

    private void AssessResult()
    {
        int theme_num = 1;
        int level_num = 1;
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

        if (score > (100f /3f))
        {
            StartCoroutine(requestsManager.AddReward("/reward", userID, 1));
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
                int next_level = 2;
                StartCoroutine(requestsManager.UpdateCurrentLevel("/users/updateLevel", next_level, userID));
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(7);
            }
        }
    }
}

