using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Q2_level2 : MonoBehaviour
{
    [Header("<---- SCENE PANELS ---->")]
    [SerializeField]
    private GameObject[] scenes = new GameObject[8];
    [SerializeField]
    private GameObject[] assessment = new GameObject[4];
    [SerializeField] private GameObject tracingO;
    [SerializeField] private GameObject tracingU;

    [Header("<---- ASSESSMENT1 GAMEOBJECTS ---->")]
    [SerializeField]
    private GameObject[] assessment1 = new GameObject[3];

    [Header("<---- ASSESSMENT2 GAMEOBJECTS ---->")]
    [SerializeField]
    private GameObject[] assessment2 = new GameObject[3];

    [Header("<---- ASSESSMENT3 GAMEOBJECTS ---->")]
    [SerializeField]
    private GameObject[] colors = new GameObject[2];
    [SerializeField]
    private Sprite[] blueSprites = new Sprite[3];
    [SerializeField]
    private Sprite[] redSprites = new Sprite[3];
    [SerializeField]
    private Image[] blueObjects = new Image[3];
    [SerializeField]
    private Image[] redObjects = new Image[3];

    [Header("<---- LESSON CONFETTI ---->")]
    [SerializeField]
    private GameObject Uconfetti;
    [SerializeField]
    private GameObject Oconfetti;

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

    [Header("<---- GAME MENU ---->")]
    [SerializeField]
    private GameObject gameMenu;

    private int objectCounter;
    private int Oo_num_of_tracing_points = 38;
    private int Uu_num_of_tracing_points = 34;

    private int assessmentScore = 100;
    private float score;
    private int error, userID;

    private Vector3[] colorsInitialPos = new Vector3[2];
    private void Start()
    {
        requestsManager = FindObjectOfType<THEME1_LEVEL1_REQUESTS>();

        userID = PlayerPrefs.GetInt("Current_user");

        PlayerPrefs.SetInt("Tracing Points", 0);
        for (int i = 0; i < 7; i++)
        {
            int index = i;
            Button next = scenes[i].GetComponentInChildren<Button>();
            next.onClick.AddListener(() => OnContinue(index));
        }
        CheckAssessment1();
        CheckAssessment2();
        CheckAssessment3();
    }

    private void Update()
    {
        if (scenes[1].activeSelf)
        {
            int traced_points = PlayerPrefs.GetInt("Tracing Points");
            if (traced_points == Oo_num_of_tracing_points)
            {
                Oconfetti.SetActive(true);
            }
        }
        else if (scenes[4].activeSelf)
        {
            int traced_points = PlayerPrefs.GetInt("Tracing Points");
            if (traced_points == Uu_num_of_tracing_points)
            {
                Uconfetti.SetActive(true);
            } 
        }

        int index = 0;

        if (gameObject.name == "Theme2_Level2 Scene Manager")
        {
            for (int i = 0; i < scenes.Length; i++)
            {
                if (scenes[i].activeSelf)
                {
                    index = i;
                }
            }

            PlayableDirector playableDirector = null;
            if (index == 7)
            {
                if (assessment[index - 7].activeSelf)
                {
                    playableDirector = assessment[index - 7].GetComponent<PlayableDirector>();
                }
                else if (assessment[index - 6].activeSelf)
                {
                    playableDirector = assessment[index - 6].GetComponent<PlayableDirector>();
                }
                else if (assessment[index - 5].activeSelf)
                {
                    playableDirector = assessment[index - 5].GetComponent<PlayableDirector>();
                }

                if (assessment[3].activeSelf)
                {
                    gameMenu.SetActive(false);
                }
                else
                {
                    if (PlayerPrefs.GetString("Paused") == "True")
                    {
                        playableDirector.Pause();
                    }
                    else
                    {
                        playableDirector.Resume();
                    }
                }
            }
            else
            {
                Debug.Log("Panel" + index);
                playableDirector = scenes[index].GetComponent<PlayableDirector>();

                if (PlayerPrefs.GetString("Paused") == "True")
                {
                    playableDirector.Pause();
                    if (index == 1)
                    {
                        tracingO.SetActive(false);
                    }
                    else if (index == 4)
                    {
                        tracingU.SetActive(false);
                    }
                }
                else
                {
                    playableDirector.Resume();
                    if (index == 1)
                    {
                        tracingO.SetActive(true);
                    }
                    else if (index == 4)
                    {
                        tracingU.SetActive(true);
                    }
                }
            }
        }
    }

    private void OnContinue(int index)
    {
        print(index);
        scenes[index].SetActive(false);
        scenes[index+1].SetActive(true);
    }

    public void OpenPreview()
    {
        scenes[0].SetActive(true);
    }

    private void CheckAssessment1()
    {
        foreach (GameObject group in assessment1)
        {
            foreach (Button item in group.GetComponentsInChildren<Button>())
            {
                item.onClick.AddListener(() => ValidateAssessmentAns(1, item.name, group, 'o'));
            }
        }
    }

    private void CheckAssessment2()
    {
        foreach (GameObject group in assessment2)
        {
            foreach (Button item in group.GetComponentsInChildren<Button>())
            {
                item.onClick.AddListener(() => ValidateAssessmentAns(2, item.name, group, 'u'));
            }
        }
    }

    private void ValidateAssessmentAns(int assessNum, string name, GameObject group, char criteria)
    {

        if ((group.name == "group1") && name[0] == criteria)
        {
            group.SetActive(false);
            if (assessNum == 1)
            {
                assessment1[1].SetActive(true);
                MoveProgress(error, 1);
            }
            else
            {
                assessment2[1].SetActive(true);
                MoveProgress(error, 2);
            }
        }
        else if ((group.name == "group2") && name[0] == criteria)
        {
            group.SetActive(false);
            if (assessNum == 1)
            {
                assessment1[2].SetActive(true);
                MoveProgress(error, 1);
            }
            else
            {
                assessment2[2].SetActive(true);
                MoveProgress(error, 2);
            }
        }
        else if ((group.name == "group3") && name[0] == criteria)
        {
            if (assessNum == 1)
            {
                assessment[0].SetActive(false);
                assessment[1].SetActive(true);
                MoveProgress(error, 1);
            }
            else
            {
                assessment[1].SetActive(false);
                assessment[2].SetActive(true);
                MoveProgress(error, 2);
            }
        }
        else
        {
            error += 6;
        }
    }

    private void CheckAssessment3()
    {
        for (int i = 0; i < 2; i++)
        {
            colorsInitialPos[i] = colors[i].transform.position;
        }
    }

    public void DragRed()
    {
        colors[0].transform.position = Input.mousePosition;
    }

    public void DragBlue()
    {
        colors[1].transform.position = Input.mousePosition;
    }

    public void DropRed()
    {
        float Distance1 = Vector3.Distance(redObjects[0].transform.position, colors[0].transform.position);
        float Distance2 = Vector3.Distance(redObjects[1].transform.position, colors[0].transform.position);
        float Distance3 = Vector3.Distance(redObjects[2].transform.position, colors[0].transform.position);

        if (Distance1 < 100)
        {
            if (redObjects[0].sprite.name != redSprites[0].name)
            {
                MoveProgress(error, 3);
                objectCounter++;
                redObjects[0].sprite = redSprites[0];
                colors[0].transform.position = colorsInitialPos[0];
            }
            else
            {
                colors[0].transform.position = colorsInitialPos[0];
            }
        }
        else if (Distance2 < 100)
        {
            if (redObjects[1].sprite != redSprites[1])
            {
                MoveProgress(error, 3);
                objectCounter++;
                redObjects[1].sprite = redSprites[1];
                colors[0].transform.position = colorsInitialPos[0];
            }
            else
            {
                colors[0].transform.position = colorsInitialPos[0];
            }
        }
        else if (Distance3 < 100)
        {
            if (redObjects[2].sprite != redSprites[2])
            {
                MoveProgress(error, 3);
                objectCounter++;
                redObjects[2].sprite = redSprites[2];
                colors[0].transform.position = colorsInitialPos[0];
            }
            else
            {
                colors[0].transform.position = colorsInitialPos[0];
            }
        }
        else
        {
            error += 3;
            colors[0].transform.position = colorsInitialPos[0];
        }

        if (objectCounter == 6)
        {
            scenes[7].SetActive(false);
            assessment[3].SetActive(true);
        }
    }

    public void DropBlue()
    {
        float Distance1 = Vector3.Distance(blueObjects[0].transform.position, colors[1].transform.position);
        float Distance2 = Vector3.Distance(blueObjects[1].transform.position, colors[1].transform.position);
        float Distance3 = Vector3.Distance(blueObjects[2].transform.position, colors[1].transform.position);

        if (Distance1 < 100)
        {
            if (blueObjects[0].sprite.name != blueSprites[0].name)
            {
                MoveProgress(error, 3);
                objectCounter++;
                blueObjects[0].sprite = blueSprites[0];
                colors[1].transform.position = colorsInitialPos[1];
            }
            else
            {
                colors[1].transform.position = colorsInitialPos[1];
            }
        }
        else if (Distance2 < 100)
        {
            if (blueObjects[1].sprite.name != blueSprites[1].name)
            {
                MoveProgress(error, 3);
                objectCounter++;
                blueObjects[1].sprite = blueSprites[1];
                colors[1].transform.position = colorsInitialPos[1];
            }
            else
            {
                colors[1].transform.position = colorsInitialPos[1];
            }
        }
        else if (Distance3 < 100)
        {
            if (blueObjects[2].sprite.name != blueSprites[2].name)
            {
                MoveProgress(error, 3);
                objectCounter++;
                blueObjects[2].sprite = blueSprites[2];
                colors[1].transform.position = colorsInitialPos[1];
            }
            else
            {
                colors[1].transform.position = colorsInitialPos[1];
            }
        }
        else
        {
            error += 3;
            colors[1].transform.position = colorsInitialPos[1];
        }

        if (objectCounter == 6)
        {
            AssessResult();
            StartCoroutine(ShowResult());
        }
    }

    IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(1f);
        assessment[2].SetActive(false);
        assessment[3].SetActive(true);
    }

    private void MoveProgress(int totalError, int assessNum)
    {
        float currentScore = 0;
        float finalAssessmentScore = 0;
        if (assessNum == 1)
        {
            Debug.Log(totalError);
            float scorePerGroup = assessmentScore / 3f;
            Debug.Log(scorePerGroup - totalError);
            finalAssessmentScore = ((float)(scorePerGroup - totalError) / assessmentScore) * (100f / 3f);
        }
        else if (assessNum == 2)
        {
            float scorePerGroup = assessmentScore / 3f;
            finalAssessmentScore = ((float)(scorePerGroup - totalError) / assessmentScore) * (100f / 3f);

        }
        else if (assessNum == 3)
        {
            float scorePerGroup = assessmentScore / 6f;
            finalAssessmentScore = ((float)(scorePerGroup - totalError) / assessmentScore) * (100f / 3f);
        }

        if (finalAssessmentScore <= 0)
        {
            currentScore = 0;
        }
        else
        {
            currentScore = finalAssessmentScore;
        }

        error = 0;

        score += currentScore;
        Debug.Log("score" + score);
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
    }

    private int delaytime;

    private void AssessResult()
    {
        int theme_num = 2;
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
            delaytime += 18;
            result[0].SetActive(true);
            result[3].SetActive(true);

            stars[0].sprite = earnedStar;
            stars[1].sprite = earnedStar;
            stars[2].sprite = earnedStar;
        }
        StartCoroutine(GoToMap());

        if (score > (100f / 3f))
        {
            StartCoroutine(requestsManager.AddReward("/reward", userID, 3));
        }
    }

    IEnumerator GoToMap()
    {
        yield return new WaitForSeconds(delaytime);
        if (score < (100f / 3f))
        {
            Debug.Log("GO BACK");
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
