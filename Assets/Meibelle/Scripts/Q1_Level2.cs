using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Q1_Level2 : MonoBehaviour
{

    //public GameObject TracingPanel;
    public GameObject Pencil;
    public GameObject PencilMask;
    public GameObject Collider;
    private Vector3 pencilState;
    private Vector3 pencilRaise = new Vector3(105, 120, 0);
    private Vector3 pencilWrite = new Vector3(85, 100, 0);

    // ----------------------------------------------------------------- //
    [SerializeField]
    private GameObject[] scenes = new GameObject[11];
    [SerializeField]
    private GameObject[] assessments = new GameObject[4];
    [SerializeField]
    private GameObject instructions, tracingpanel;
    [SerializeField]
    private Button tracing_button;
    

    [SerializeField]
    private Button[] exercise1 = new Button[8];
    [SerializeField]
    private Button[] exercise2 = new Button[2];

    [SerializeField]
    private Button treasureBox;
    [SerializeField]
    private GameObject[] assessment2 = new GameObject[4];
    Vector3[] keyInitialPos = new Vector3[3];
    [SerializeField]
    private GameObject[] assessment3 = new GameObject[2];
    [SerializeField]
    private GameObject[] ballons = new GameObject[2];
    [SerializeField]
    private GameObject sticker;

    [SerializeField]
    private Image progressBar;
    [SerializeField]
    private GameObject[] result = new GameObject[5];

    int num_of_tracing_points = 39;
    int assess1 = 50;
    int assess2 = 25;
    int assess3 = 25;
    int error;
    float score;
    int userID;

    private void Start()
    {
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

        foreach (GameObject balloon in ballons)
        {
            Button button = balloon.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => CheckAssessment3(balloon));
        }

        tracing_button.onClick.AddListener(() => CheckAssessment1());
    }

    public void OnContinue(int index)
    {
        scenes[index].SetActive(false);
        scenes[index + 1].SetActive(true);
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
                print("wewo");
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
        //assessments[1].SetActive(true);
    }

    private void CheckAssessment1()
    {
        int points = PlayerPrefs.GetInt("Tracing Points");
        float score_per_point = 50f / (float) num_of_tracing_points;
        assessments[0].SetActive(false);
        assessments[1].SetActive(true);
        //Debug.Log(score_per_point);
        //PlayerPrefs.SetFloat("tracing score", score_per_point * points);
        score += score_per_point * points;
        progressBar.fillAmount = (score_per_point * points) / 100;
        //MoveProgress(error, 1);
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
            error += 12;
            key.transform.position = keyInitialPos[0];
        }
        else if (key.name == "key-e")
        {
            error += 12;
            key.transform.position = keyInitialPos[1];
        }
        else if (key.name == "key-h")
        {
            error += 12;
            key.transform.position = keyInitialPos[2];
        }
    }

    IEnumerator DelayAssessment3()
    {
        yield return new WaitForSeconds(1.5f);
        assessments[1].SetActive(false);
        assessments[2].SetActive(true);
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
            error += 25;
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
            if (assess1 > totalError)
            {
                currentScore = assess1 - totalError;
            }
            else
            {
                currentScore = 0;
            }
        }
        else if (assessNum == 2)
        {
            if (assess2 > totalError)
            {
                currentScore = assess2 - totalError;
            }
            else
            {
                currentScore = 0;
            }
        }
        else if (assessNum == 3)
        {
            if (assess3 > totalError)
            {
                currentScore = assess3 - totalError;
            }
            else
            {
                currentScore = 0;
            }
        }
        error = 0;

        score += currentScore;
        print("score" + score);
        progressBar.fillAmount = score / 100;
    }

    int delaytime;
    private void AssessResult()
    {
        StartCoroutine(UpdateCurrentScore());
        if (score < 50)
        {
            result[4].SetActive(true);
        }
        else
        {

            if (score >= 50 && score < 75)
            {
                delaytime = 4;
                result[1].SetActive(true);
            }
            else if (score >= 75 && score < 100)
            {
                delaytime = 4;
                result[0].SetActive(true);
                result[2].SetActive(true);
            }
            else
            {
                delaytime = 8;
                result[0].SetActive(true);
                result[3].SetActive(true);
            }
            StartCoroutine(GoToMap());
        }
        
    }

    public void ResultButton(Button buttonType)
    {
        if (buttonType.name == "retry-button")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(10);
        }
    }

    IEnumerator GoToMap()
    {
        yield return new WaitForSeconds(delaytime);
        StartCoroutine(UpdateCurrentLevel());
    }

    IEnumerator UpdateCurrentLevel()
    {
        int current_level = 3;
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

    IEnumerator UpdateCurrentScore()
    {
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"theme_num\": 1, \"level_num\": 1, \"score\": " + score + "}");

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
}


