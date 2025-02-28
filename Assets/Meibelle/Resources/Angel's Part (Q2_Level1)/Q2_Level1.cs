using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Q2_Level1 : MonoBehaviour
{
    [Header("<---- SCENE PANELS ---->")]
    [SerializeField]
    private GameObject[] scenes = new GameObject[9];
    [SerializeField]
    private GameObject[] assessments = new GameObject[4];

    [Header("<---- EXERCISE1 GAMEOBJECTS ---->")]
    [SerializeField]
    private GameObject[] exercise1 = new GameObject[4];

    [Header("<---- ASSESSMENT1 GAMEOBJECTS ---->")]
    [SerializeField]
    private GameObject[] assessment1Option = new GameObject[4];
    [SerializeField]
    private GameObject[] assessment1Destination = new GameObject[4];
    Vector3[] optionInitialPos = new Vector3[4];
    [SerializeField]
    private GameObject confetti;

    [Header("<---- ASSESSMENT3 GAMEOBJECTS ---->")]
    [SerializeField]
    private Button[] assessment3Buttons = new Button[6];
    [SerializeField]
    private GameObject[] assessment3Instructions = new GameObject[3];
    [SerializeField]
    private GameObject assess3confetti;

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

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private THEME1_LEVEL1_REQUESTS requestsManager;

    int assess1 = 100;
    int assess2 = 100;
    int assess3 = 100;
    int error;
    int userID;
    float score;

    private void Start()
    {
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
            int index = i;
            Button option = exercise1[i].GetComponentInChildren<Button>();
            if (option.name == "circle")
            {
                option.onClick.AddListener(() => ShowConfetti());
            }
        }

        for (int i = 0; i < assessment1Option.Length; i++)
        {
            optionInitialPos[i] = assessment1Option[i].transform.position;
        }

        for (int i = 0; i < assessment3Buttons.Length; i++)
        {
            int index = i;
            assessment3Buttons[i].onClick.AddListener(() => CheckAssessment3(assessment3Buttons[index]));
        }
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("MatchingType Score"))
        {
            int Matchingscore = PlayerPrefs.GetInt("MatchingType Score");
            error = 25 - Matchingscore;
            MoveProgress(error, 2);
            Debug.Log(Matchingscore);
        }
        PlayerPrefs.DeleteKey("MatchingType Score");

    }

    public void OpenPreview()
    {
        scenes[0].SetActive(true);
    }

    public void OnContinue(int index)
    {
        scenes[index].SetActive(false);
        scenes[index + 1].SetActive(true);
    }

    private void ShowConfetti()
    {
        exercise1[3].SetActive(true);
    }

    public void NextAssessment(int index)
    {
        assessments[index].SetActive(false);
        assessments[index + 1].SetActive(true);
        if (index == 2)
        {
            AssessResult();
        }
    }

    public void DragMember(GameObject member)
    {
        member.transform.position = Input.mousePosition;
    }

    public void DropMember(GameObject member)
    {
        int index = System.Array.IndexOf(assessment1Option, member);
        float Distance = Vector3.Distance(member.transform.position, assessment1Destination[index].transform.position);
        Debug.Log(Distance);
        if (Distance < 100)
        {
            member.transform.position = assessment1Destination[index].transform.position;
            StartCoroutine(AfterDragDrop());
            MoveProgress(error, 1);
        }
        else
        {
            member.transform.position = optionInitialPos[index];
            error += 8;
        }
    }

    IEnumerator AfterDragDrop()
    {
        yield return new WaitForSeconds(1.5f);
        int positionedMembers = 0;

        for (int i = 0; i < assessment1Option.Length; i++)
        {
            if (assessment1Option[i].transform.position == assessment1Destination[i].transform.position)
            {
                positionedMembers++;
            }
        }

        if (positionedMembers == 4)
        {
            confetti.SetActive(true);
            
        }
    }

    private void CheckAssessment3(Button button)
    {
        if (assessment3Instructions[0].activeSelf)
        {
            if (button.name == "Ball")
            {
                button.interactable = false;
                assessment3Instructions[0].SetActive(false);
                assessment3Instructions[1].SetActive(true);
            }
            else
            {
                error += 6;
            }
        }
        else if (assessment3Instructions[1].activeSelf)
        {
            if (button.name == "Pinetree")
            {
                button.interactable = false;
                assessment3Instructions[1].SetActive(false);
                assessment3Instructions[2].SetActive(true);
            }
            else
            {
                error += 6;
            }
        }
        else
        {
            if (button.name == "Dice")
            {
                button.interactable = false;
                assess3confetti.SetActive(true);
            }
            else
            {
                error += 6;
            }
        }
        MoveProgress(error, 3);
    }

    private void MoveProgress(int totalError, int assessNum)
    {
        float currentScore = 0;
        if (assessNum == 1)
        {
            float scorePerGroup = assess1 / 4f;
            float a1 = ((float)(scorePerGroup - totalError) / assess1) * (100f / 3f);

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
            float scorePerGroup = assess2 / 4f;
            float a2 = ((float)(scorePerGroup - totalError) / assess2) * (100f / 3f);

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
            float scorePerGroup = assess3 / 3f;
            float a3 = ((float)(scorePerGroup - totalError) / assess3) * (100f / 3f);

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
        int theme_num = 2;
        int level_num = 1;
        StartCoroutine(requestsManager.UpdateCurrentScore("/scores", score, userID, theme_num, level_num));

        float star1 = (100f / 3f);
        float star2 = (100f / 3f) * 2;
        float star3 = (100f / 3f) * 3;

        if (score < star1)
        {
            result[4].SetActive(true);
            delaytime = 4;
        }
        else if (score >= star1 && score < star2)
        {
            result[1].SetActive(true);
            delaytime = 4;
        }
        else if (score >= star2 && score < star3)
        {
            delaytime = 4;
            result[0].SetActive(true);
            result[2].SetActive(true);
        }
        else
        {
            delaytime += 8;
            result[0].SetActive(true);
            result[3].SetActive(true);
        }
        StartCoroutine(GoToMap());
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
            int next_level = 2;
            StartCoroutine(requestsManager.UpdateCurrentLevel("/users/updateLevel", next_level, userID));
        }
    }
}
