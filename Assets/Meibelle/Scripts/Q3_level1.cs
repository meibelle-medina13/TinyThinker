using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.TextCore.Text;
using UnityEngine.Playables;
using System.Reflection;

public class Q3_level1 : MonoBehaviour
{
    [Header("<---- SCENE PANELS ---->")]
    [SerializeField]
    private GameObject[] scenes = new GameObject[10];
    [SerializeField]
    private GameObject[] assessments = new GameObject[4];

    [Header("<---- EXERCISE 1 ---->")]
    [SerializeField]
    private GameObject tracingpanel1;
    [SerializeField]
    private GameObject maraIcon;
    [SerializeField]
    private GameObject pencil;

    [Header("<---- EXERCISE 2 ---->")]
    [SerializeField]
    private GameObject[] flowers = new GameObject[5];
    [SerializeField]
    private GameObject waterSprinker;
    [SerializeField]
    private GameObject waterDrops;
    private Vector3 sprinklerInitialPos;

    [Header("<---- EXERCISE 3 ---->")]
    [SerializeField]
    private GameObject tracingpanel2;
    [SerializeField]
    private GameObject maraIcon2;
    [SerializeField]
    private GameObject pencil2;

    [Header("<---- EXERCISE 4 ---->")]
    [SerializeField]
    private GameObject[] leaves = new GameObject[6];
    private Vector3[] leavesInitialPos = new Vector3[6];
    [SerializeField]
    private GameObject driedLeaves;
    private int leafCounter = 0;
    [SerializeField]
    private GameObject confetti;

    [Header("<---- ASSESSMENT 1 ---->")]
    [SerializeField]
    private GameObject[] characters = new GameObject[2];
    private Vector3[] charInitialPos = new Vector3[2];
    [SerializeField]
    private GameObject[] setting = new GameObject[2];
    [SerializeField]
    private GameObject[] timelines = new GameObject[2];
    [SerializeField]
    private GameObject[] assess1_colliders = new GameObject[2];
    [SerializeField]
    private GameObject assess1Confetti;
    private int characterCounter;


    [Header("<---- ASSESSMENT 2 ---->")]
    [SerializeField]
    private GameObject tracingpanel3;
    [SerializeField]
    private GameObject pencil3;

    [Header("<---- ASSESSMENT 3 ---->")]
    [SerializeField]
    private GameObject[] puzzlePieces = new GameObject[7];
    [SerializeField]
    private GameObject[] puzzleBG = new GameObject[7];
    private Vector3[] piecesInitialPos = new Vector3[7];
    [SerializeField]
    private GameObject puzzlePattern;
    [SerializeField]
    private GameObject[] assess3_colliders = new GameObject[7];
    [SerializeField]
    private GameObject confetti3;
    private int puzzleCounter;

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

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audioClip = new AudioClip[4];

    [Header("<---- GAME MENU ---->")]
    [SerializeField]
    private GameObject gameMenu;

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private THEME1_LEVEL1_REQUESTS requestsManager;

    private int error, delaytime, userID;
    private int assessmentScore = 100;
    private float score;


    private void Start()
    {
        //DELETE
        PlayerPrefs.DeleteKey("CurrentPanel");
        //DELETE
        PlayerPrefs.SetInt("Tracing Points", 0);
        PlayerPrefs.SetInt("Wrong Points", 0);
        PlayerPrefs.SetString("Collider", "");

        requestsManager = FindObjectOfType<THEME1_LEVEL1_REQUESTS>();


        for (int i = 0; i < (scenes.Length - 1); i++)
        {
            int index = i;
            Button nextButton = scenes[i].GetComponentInChildren<Button>();
            if (nextButton != null && nextButton.name == "next-button")
            {
                nextButton.onClick.AddListener(() => OnContinue(index));
            }
        }

        sprinklerInitialPos = waterSprinker.transform.position;

        for (int i = 0; i < leaves.Length; i++)
        {
            leavesInitialPos[i] = leaves[i].transform.position;
        }

        for (int i = 0; i < characters.Length; i++)
        {
            charInitialPos[i] = characters[i].transform.position;
        }

        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            piecesInitialPos[i] = puzzlePieces[i].transform.position;
        }
    }

    public void OpenPreview()
    {
        scenes[0].SetActive(true);
        PlayerPrefs.SetInt("CurrentPanel", 0);
    }

    public void OpenTracing(int tracingnum)
    {
        if (tracingnum == 1)
        {
            tracingpanel1.SetActive(true);
        }
        else if (tracingnum == 2)
        {
            tracingpanel2.SetActive(true);
        }
        else if (tracingnum == 3)
        {
            tracingpanel3.SetActive(true);
        }
    }

    private void Update()
    {
        if (scenes[1].activeSelf)
        {
            if (PlayerPrefs.GetInt("Tracing Points") == 7)
            {
                maraIcon.SetActive(true);
                pencil.SetActive(false);
                PlayerPrefs.SetInt("Tracing Points", 0);
            }
        }
        else if (scenes[3].activeSelf)
        {
            string colliderName = PlayerPrefs.GetString("Collider");
            if (colliderName != "")
            {
                PlayerPrefs.SetString("Collider", "");
                DropSprinkler(colliderName);
            }
        }
        else if (scenes[4].activeSelf)
        {
            if (PlayerPrefs.GetInt("Tracing Points") == 16)
            {
                maraIcon2.SetActive(true);
                pencil2.SetActive(false);
                PlayerPrefs.SetInt("Tracing Points", 0);
            }
        }

        PlayableDirector playableDirector;

        int index = PlayerPrefs.GetInt("CurrentPanel");
        if (!scenes[9].activeSelf)
        {
            playableDirector = scenes[index].GetComponent<PlayableDirector>();
        }
        else
        {
            playableDirector = assessments[index].GetComponent<PlayableDirector>();
        }

        if (PlayerPrefs.GetString("Paused") == "True")
        {
            playableDirector.Pause();
            if (tracingpanel1.activeSelf)
            {
                tracingpanel1.SetActive(false);
            }
            else if (tracingpanel2.activeSelf)
            {
                tracingpanel2.SetActive(false);
            }
            else if (tracingpanel3.activeSelf)
            {
                tracingpanel3.SetActive(false);
            }
        }
        else if (PlayerPrefs.GetString("Paused") == "False")
        {
            playableDirector.Resume();
            PlayerPrefs.DeleteKey("Paused");
            if (playableDirector.time == 0)
            {
                if (scenes[1].activeSelf)
                {
                    tracingpanel1.SetActive(true);
                }
                else if (scenes[4].activeSelf)
                {
                    tracingpanel2.SetActive(true);
                }
                else if (assessments[1].activeSelf)
                {
                    tracingpanel3.SetActive(true);
                }
            }
        }

    }

    public void OnContinue(int index)
    {
        scenes[index].SetActive(false);
        scenes[index + 1].SetActive(true);
        if (index == 8)
        {
            PlayerPrefs.SetInt("CurrentPanel", 0);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentPanel", index + 1);
        }
    }

    public void DragSprinker(GameObject sprinkler)
    {
        sprinkler.transform.position = Input.mousePosition;
    }

    private void DropSprinkler(string colliderName)
    {
        if (colliderName == "forF1")
        {
            waterSprinker.transform.Rotate(0, 0, -47);
            if (flowers[2].activeSelf)
            {
                StartCoroutine(ShowFlowers(3));
            }
            else
            {
                StartCoroutine(ShowFlowers(0));
            }
        }
        else if (colliderName == "forF2")
        {
            waterSprinker.transform.Rotate(0, 0, -47);
            StartCoroutine(ShowFlowers(1));
        }
        else if (colliderName == "forF3")
        {
            waterSprinker.transform.localScale = new Vector3(-1, 1, 1);
            waterSprinker.transform.Rotate(0, 0, 47);
            waterSprinker.transform.localPosition = new Vector3(-358, 199, -90);

            if (flowers[0].activeSelf)
            {
                StartCoroutine(ShowFlowers(3));
            }
            else
            {
                StartCoroutine(ShowFlowers(2));
            }
        }
    }

    IEnumerator ShowFlowers(int flowerIndex)
    {
        waterDrops.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        audioSource.PlayOneShot(audioClip[0]);
        flowers[flowerIndex].SetActive(true);

        waterSprinker.transform.position = sprinklerInitialPos;
        waterDrops.SetActive(false);

        if (waterSprinker.transform.localScale.x == -1)
        {
            waterSprinker.transform.Rotate(0, 0, -47);
            waterSprinker.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            waterSprinker.transform.Rotate(0, 0, 47);
        }

        if (flowers[3].activeSelf && flowers[1].activeSelf)
        {
            flowers[4].SetActive(true);
        }

    }

    public void DragLeaf(GameObject leaf)
    {
        leaf.transform.position = Input.mousePosition;
    }

    public void DropLeaf(GameObject leaf)
    {
        float Distance = Vector3.Distance(leaf.transform.position, driedLeaves.transform.position);
        int index = int.Parse(leaf.name.Substring(10, 1));
        if (Distance <= 250)
        {
            leaf.transform.position = Input.mousePosition;
            leafCounter++;

            if (leafCounter == 6)
            {
                confetti.SetActive(true);
            }
        }
        else
        {
            leaf.transform.position = leavesInitialPos[index];
        }
    }

    public void OpenGift(GameObject gift)
    {
        gift.SetActive(true);
    }

    public void DragCharacter(GameObject character)
    {
        character.transform.position = Input.mousePosition;
    }

    public void DropCharacter(GameObject character)
    {
        string colliderName = PlayerPrefs.GetString("Collider");
        if (character.name == "mr-marley" && colliderName == "garden_collider")
        {
            PlayerPrefs.SetString("Collider", "");
            timelines[0].SetActive(true);
            MoveProgress(error, 1);
            characterCounter++;
            audioSource.PlayOneShot(audioClip[1]);
        }
        else if (character.name == "mrs-loti" && colliderName == "backyard_collider")
        {
            PlayerPrefs.SetString("Collider", "");
            timelines[1].SetActive(true);
            MoveProgress(error, 1);
            characterCounter++;
            audioSource.PlayOneShot(audioClip[1]);
        }
        else
        {
            if (character.name == "mr-marley")
            {
                character.transform.position = charInitialPos[0];
                error += 50;
                assess1_colliders[0].SetActive(true);
            }
            else
            {
                character.transform.position = charInitialPos[1];
                error += 50;
                assess1_colliders[1].SetActive(true);
            }
            audioSource.PlayOneShot(audioClip[2]);
        }

        if (characterCounter == 2)
        {
            assess1Confetti.SetActive(true);
            audioSource.PlayOneShot(audioClip[3]);
            StartCoroutine(OpenAssessment2(0));
        }
    }

    IEnumerator OpenAssessment2(int current)
    {
        yield return new WaitForSeconds(3);
        assessments[current].SetActive(false);
        assessments[current+1].SetActive(true);
        PlayerPrefs.SetInt("CurrentPanel", 1);
    }

    private void CheckAssessment2()
    {
        int wrongPoints = PlayerPrefs.GetInt("Wrong Points");
        MoveProgress(wrongPoints, 2);
    }

    public void OpenAssessment3()
    {
        CheckAssessment2();
        assessments[1].SetActive(false);
        assessments[2].SetActive(true);
        PlayerPrefs.SetInt("CurrentPanel", 2);
    }

    public void OpenResult()
    {
        scenes[9].SetActive(false);
        assessments[3].SetActive(true);
        AssessResult();
    }

    public void DragPuzzle(GameObject piece)
    {
        int index = int.Parse(piece.name.Substring(6, 1));
        piece.transform.position = Input.mousePosition;
    }

    public void DropPuzzle(GameObject piece)
    {
        string colliderName = PlayerPrefs.GetString("Collider");
        int index = int.Parse(piece.name.Substring(6, 1));

        if (colliderName == "p1" && index == 1)
        {
            piece.SetActive(false);
            puzzleBG[index - 1].SetActive(true);
            puzzleCounter++;
            MoveProgress(error, 3);
            audioSource.PlayOneShot(audioClip[1]);
        }
        else if (colliderName == "p2" && index == 2)
        {
            piece.SetActive(false);
            puzzleBG[index - 1].SetActive(true);
            puzzleCounter++;
            MoveProgress(error, 3);
            audioSource.PlayOneShot(audioClip[1]);
        }
        else if (colliderName == "p3" && index == 3)
        {
            piece.SetActive(false);
            puzzleBG[index - 1].SetActive(true);
            puzzleCounter++;
            MoveProgress(error, 3);
            audioSource.PlayOneShot(audioClip[1]);
        }
        else if (colliderName == "p4" && index == 4)
        {
            piece.SetActive(false);
            puzzleBG[index - 1].SetActive(true);
            puzzleCounter++;
            MoveProgress(error, 3);
            audioSource.PlayOneShot(audioClip[1]);
        }
        else if (colliderName == "p5" && index == 5)
        {
            piece.SetActive(false);
            puzzleBG[index - 1].SetActive(true);
            puzzleCounter++;
            MoveProgress(error, 3);
            audioSource.PlayOneShot(audioClip[1]);
        }
        else if (colliderName == "p6" && index == 6)
        {
            piece.SetActive(false);
            puzzleBG[index - 1].SetActive(true);
            puzzleCounter++;
            MoveProgress(error, 3);
            audioSource.PlayOneShot(audioClip[1]);
        }
        else if (colliderName == "p7" && index == 7)
        {
            piece.SetActive(false);
            puzzleBG[index - 1].SetActive(true);
            puzzleCounter++;
            MoveProgress(error, 3);
            audioSource.PlayOneShot(audioClip[1]);
        }
        else
        {
            piece.transform.position = piecesInitialPos[index-1];
            error += 10;
            audioSource.PlayOneShot(audioClip[2]);
        }

        for (int i = 0; i < assess3_colliders.Length; i++)
        {
            assess3_colliders[i].SetActive(true);
        }

        if (puzzleCounter == 7)
        {
            for (int i = 0; i < puzzleBG.Length; i++)
            {
                puzzleBG[i].SetActive(false);
            }
            confetti3.SetActive(true);
            audioSource.PlayOneShot(audioClip[3]);
        }
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
            int points = PlayerPrefs.GetInt("Tracing Points");
            int num_of_tracing_points = 11;
            float scorePerGroup = assessmentScore / (float) num_of_tracing_points;
            finalAssessmentScore = ((float)(scorePerGroup * (points - totalError)) / assessmentScore) * (100f / 3f);

        }
        else if (assessNum == 3)
        {
            float scorePerGroup = assessmentScore / 7f;
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

    private void AssessResult()
    {
        gameMenu.SetActive(false);
        int theme_num = 3;
        int level_num = 1;
        userID = PlayerPrefs.GetInt("Current_user");

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
        else if (score > 99.9f || score == star3)
        {
            delaytime = 8;
            result[0].SetActive(true);
            result[3].SetActive(true);
        }
        else if (score >= star2 && score < star3)
        {
            delaytime = 4;
            result[0].SetActive(true);
            result[2].SetActive(true);
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
