using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
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

    [Header("<---- ASSESSMENT2 GAMEOBJECTS ---->")]
    [SerializeField] private GameObject characters;
    [SerializeField] private GameObject things;

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

    [Header("<---- SOUND EFFECTS ---->")]
    [SerializeField]
    private AudioSource SFX;
    [SerializeField]
    private AudioClip[] audioClips = new AudioClip[2];

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private THEME1_LEVEL1_REQUESTS requestsManager;

    [Header("<---- GAME MENU ---->")]
    [SerializeField]
    private GameObject gameMenu;

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

    private void PlaySFX(int index)
    {
        SFX.clip = audioClips[index];
        SFX.Play();
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("MatchingType Score"))
        {
            int Matchingscore = PlayerPrefs.GetInt("MatchingType Score");
            error = 25 - Matchingscore;
            PlaySFX(1);
            MoveProgress(error, 2);
            Debug.Log(Matchingscore);
        }
        PlayerPrefs.DeleteKey("MatchingType Score");

        int index = 0;

        if (gameObject.name == "Q2_Level1 Scene Manager")
        {
            for (int i = 0; i < scenes.Length; i++)
            {
                if (scenes[i].activeSelf)
                {
                    index = i;
                }
            }

            PlayableDirector playableDirector = null;
            if (index == 8)
            {
                if (assessments[index-8].activeSelf)
                {
                    playableDirector = assessments[index - 8].GetComponent<PlayableDirector>();
                }
                else if (assessments[index-7].activeSelf)
                {
                    playableDirector = assessments[index - 7].GetComponent<PlayableDirector>();
                }
                else if (assessments[index - 6].activeSelf)
                {
                    for (int j = 0; j < assessment3Instructions.Length; j++)
                    {
                        if (assessment3Instructions[j].activeSelf)
                        {
                            playableDirector = assessment3Instructions[j].GetComponent<PlayableDirector>();
                            break;
                        }
                    }
                }

                if (assessments[3].activeSelf)
                {
                    gameMenu.SetActive(false);
                }
                else
                {
                    if (PlayerPrefs.GetString("Paused") == "True")
                    {
                        playableDirector.Pause();
                        if (assessments[1].activeSelf)
                        {
                            characters.SetActive(false);
                            things.SetActive(false);
                        }
                    }
                    else
                    {
                        playableDirector.Resume();
                        if (assessments[1].activeSelf)
                        {
                            characters.SetActive(true);
                            things.SetActive(true);
                        }
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
                }
                else
                {
                    playableDirector.Resume();
                }
            }
        }
    }

    public void OpenPreview()
    {
        scenes[0].SetActive(true);
    }

    public void OnContinue(int index)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            selectedButton.enabled = false;
        }
        scenes[index].SetActive(false);
        scenes[index + 1].SetActive(true);
    }

    private void ShowConfetti()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            selectedButton.enabled = false;
        }
        exercise1[3].SetActive(true);
    }

    public void NextAssessment(int index)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            selectedButton.enabled = false;
        }
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
        Debug.Log(assessment1Option[index].name);
        for (int i = 0; i < assessment1Destination.Length; i++)
        {
            //Debug.Log(assessment1Destination[i].name);
            float Distance = Vector3.Distance(member.transform.position, assessment1Destination[i].transform.position);
            //Debug.Log(Distance);
            if (Distance < 100)
            {
                if (assessment1Option[index].name == assessment1Option[i].name)
                {
                    Debug.Log(assessment1Option[index].name + " " + assessment1Option[i].name);
                    member.SetActive(false);
                    Color bearColor = assessment1Destination[i].GetComponent<Image>().color;
                    bearColor.a = 1f;
                    assessment1Destination[i].GetComponent<Image>().color = bearColor;
                    StartCoroutine(AfterDragDrop());
                    PlaySFX(1);
                    MoveProgress(error, 1);
                    break;
                }
                else
                {
                    member.transform.position = optionInitialPos[index];
                    error += 8;
                    PlaySFX(0);
                    break;
                }
            }
            else if (Distance >= 100 && i == assessment1Destination.Length - 1)
            {
                member.transform.position = optionInitialPos[index];
            }
            //else
            //{
            //    member.transform.position = optionInitialPos[index];
            //}
            //if (member.name == assessment1Option[i].name && Distance < 100)
            //{
            //    member.transform.position = assessment1Destination[i].transform.position;
            //    StartCoroutine(AfterDragDrop());
            //    PlaySFX(1);
            //    MoveProgress(error, 1);
            //}
            //else if (member.name != assessment1Option[i].name && Distance < 100)
            //{
            //    member.transform.position = optionInitialPos[index];
            //    error += 8;
            //    PlaySFX(0);
            //}
            //else
            //{
            //    member.transform.position = optionInitialPos[index];
            //}
        }
    }

    IEnumerator AfterDragDrop()
    {
        yield return new WaitForSeconds(0.5f);
        int positionedMembers = 0;

        for (int i = 0; i < assessment1Option.Length; i++)
        {
            if (!assessment1Option[i].activeSelf)
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
                MoveProgress(error, 3);
                PlaySFX(1);
            }
            else
            {
                error += 6;
                PlaySFX(0);
            }
        }
        else if (assessment3Instructions[1].activeSelf)
        {
            if (button.name == "Pinetree")
            {
                button.interactable = false;
                assessment3Instructions[1].SetActive(false);
                assessment3Instructions[2].SetActive(true);
                MoveProgress(error, 3);
                PlaySFX(1);
            }
            else
            {
                error += 6;
                PlaySFX(0);
            }
        }
        else
        {
            if (button.name == "Dice")
            {
                button.interactable = false;
                assess3confetti.SetActive(true);
                MoveProgress(error, 3);
                PlaySFX(1);
            }
            else
            {
                error += 6;
                PlaySFX(0);
            }
        }
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
            delaytime = 7;
        }
        else if (score >= star1 && score < star2)
        {
            result[1].SetActive(true);
            delaytime = 6;
        }
        else if (score >= star2 && score < star3)
        {
            delaytime = 6;
            result[0].SetActive(true);
            result[2].SetActive(true);
        }
        else
        {
            delaytime += 13;
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
