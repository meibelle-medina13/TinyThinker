using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Q2_level3 : MonoBehaviour
{
    [Header("<---- SCENE PANELS ---->")]
    [SerializeField]
    private GameObject[] scenes = new GameObject[13];

    [Header("<---- ASSESSMENT GAMEOBJECTS ---->")]
    [SerializeField]
    private GameObject[] assessment1 = new GameObject[3];
    [SerializeField] 
    private GameObject[] assessment2 = new GameObject[4];
    [SerializeField] 
    private GameObject[] assessment3 = new GameObject[3];

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

    int counter, userID;
    int assessmentScore = 100;
    float score;
    int error = 0;

    private void Start()
    {
        requestsManager = FindObjectOfType<THEME1_LEVEL1_REQUESTS>();

        userID = PlayerPrefs.GetInt("Current_user");

        CheckAssessment1();
        CheckAssessment2();
        CheckAssessment3();
    }

    private void Update()
    {
        int index = 0;

        if (gameObject.name == "THEME2_Level3 Scene Manager")
        {
            for (int i = 0; i < scenes.Length; i++)
            {
                if (scenes[i].activeSelf)
                {
                    index = i;
                }
            }

            PlayableDirector playableDirector = null;
            if (index == 12)
            {
                gameMenu.SetActive(false);
            }
            else if (index == 11)
            {
                if (assessment1[0].activeSelf)
                {
                    for (int j = 1; j < assessment1.Length; j++)
                    {
                        if (assessment1[j].activeSelf)
                        {
                            playableDirector = assessment1[j].GetComponent<PlayableDirector>();
                            break;
                        }
                    }
                }
                else if (assessment2[0].activeSelf)
                {
                    playableDirector = assessment2[0].GetComponent<PlayableDirector>();
                }
                else if (assessment3[0].activeSelf)
                {
                    playableDirector = assessment3[0].GetComponent<PlayableDirector>();
                }

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
                Debug.Log("Panel" + index);
                playableDirector = scenes[index].GetComponent<PlayableDirector>();

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
    }

    public void onContinue()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            selectedButton.enabled = false;
        }

        if (counter <= 11)
        {
            scenes[counter].SetActive(false);
            counter++;
            scenes[counter].SetActive(true);
        }
    }

    public void OpenPreview()
    {
        scenes[0].SetActive(true);
    }

    private void CheckAssessment1()
    {
        for (int i = 1; i <= 2; i++)
        {
            GameObject group = assessment1[i];
            foreach (Button item in group.GetComponentsInChildren<Button>())
            {
                item.onClick.AddListener(() => validateAssessmentAns(item.name, group, 1));
            }
        }
    }

    private void validateAssessmentAns(string name, GameObject gameObject, int assessNum)
    {
        float prev = score;

        if (name == "left-arrow" && gameObject.name == "group1")
        {
            gameObject.SetActive(false);
            assessment1[2].SetActive(true);
            MoveProgress(error, 1);
        }
        else if (name == "right-arrow" && gameObject.name == "group2")
        {
            assessment1[0].SetActive(false);
            assessment2[0].SetActive(true);
            MoveProgress(error, 1);
        }
        else if (name == "triangle" && gameObject.name == "Assessment2")
        {
            assessment2[0].SetActive(false);
            assessment3[0].SetActive(true);
            MoveProgress(error, 2);
        }
        else if(name == "good" && gameObject.name == "group1")
        {
            assessment3[1].SetActive(false);
            assessment3[2].SetActive(true);
            MoveProgress(error, 3);
        }
        else if(name == "good" && gameObject.name == "group2")
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                selectedButton.enabled = false;
            }

            MoveProgress(error, 3);
            StartCoroutine(ShowResult());
        }
        else
        {
            error += 50;
        }
    }

    private void CheckAssessment2()
    {
        for (int i = 1; i <= 3; i++)
        {
            Button option = assessment2[i].GetComponentInChildren<Button>();
            option.onClick.AddListener(() => validateAssessmentAns(option.name, assessment2[0], 2));
        }
    }

    private void CheckAssessment3()
    {
        for (int i = 1; i <= 2; i++)
        {
            GameObject group = assessment3[i];
            foreach (Button item in group.GetComponentsInChildren<Button>())
            {
                item.onClick.AddListener(() => validateAssessmentAns(item.name, group, 3));
            }
        }
    }

    IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(1);
        scenes[11].SetActive(false);
        scenes[12].SetActive(true);
        AssessResult();
    }

    private void MoveProgress(int totalError, int assessNum)
    {
        float currentScore = 0;
        float finalAssessmentScore = 0;
        if (assessNum == 1)
        {
            float scorePerGroup = assessmentScore / 2f;
            finalAssessmentScore = ((float)(scorePerGroup - totalError) / assessmentScore) * (100f / 3f);
        }
        else if (assessNum == 2)
        {
            float scorePerGroup = assessmentScore / 1f;
            finalAssessmentScore = ((float)(scorePerGroup - totalError) / assessmentScore) * (100f / 3f);

        }
        else if (assessNum == 3)
        {
            float scorePerGroup = assessmentScore / 2f;
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
        progressBar.fillAmount = score / 100;
        if (score >= (100f / 3f) * 1 && score < (100f / 3f) * 2)
        {
            stars[0].sprite = earnedStar;
        }
        else if (score > 99.9f || score == (100f / 3f) * 3)
        {
            stars[0].sprite = earnedStar;
            stars[1].sprite = earnedStar;
            stars[2].sprite = earnedStar;
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
        int level_num = 3;

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
        else if (score > 99.9f || score == star3)
        {
            delaytime = 18;
            result[0].SetActive(true);
            result[3].SetActive(true);
        }
        else if (score >= star2 && score < star3)
        {
            delaytime += 12;
            result[0].SetActive(true);
            result[2].SetActive(true);
        }
        StartCoroutine(GoToMap());

        if (score > (100f / 3f))
        {
            StartCoroutine(requestsManager.AddReward("/reward", userID, 4));
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
