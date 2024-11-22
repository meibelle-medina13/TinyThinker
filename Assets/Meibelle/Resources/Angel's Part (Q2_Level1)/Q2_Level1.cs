using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Q2_Level1 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] scenes = new GameObject[9];
    [SerializeField]
    private GameObject[] assessments = new GameObject[4];

    [SerializeField]
    private GameObject[] exercise1 = new GameObject[4];

    [SerializeField]
    private GameObject[] assessment1Option = new GameObject[4];
    [SerializeField]
    private GameObject[] assessment1Destination = new GameObject[4];
    Vector3[] optionInitialPos = new Vector3[4];
    [SerializeField]
    private GameObject confetti;

    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private GameObject[] result = new GameObject[5];

    int assess1 = 100;
    int error;
    float score;

    private void Start()
    {
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
    }

    public void DragMember(GameObject member)
    {
        member.transform.position = Input.mousePosition;
    }

    public void DropMember(GameObject member)
    {
        int index = System.Array.IndexOf(assessment1Option, member);
        float Distance = Vector3.Distance(member.transform.position, assessment1Destination[index].transform.position);

        if (Distance < 50)
        {
            member.transform.position = assessment1Destination[index].transform.position;
            StartCoroutine(AfterDragDrop());
            MoveProgress(error, 1);
        }
        else
        {
            member.transform.position = optionInitialPos[index];
            error += 12;
            
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
            assessments[0].SetActive(false);
            assessments[3].SetActive(true);
            AssessResult();
        }
    }

    private void MoveProgress(int totalError, int assessNum)
    {
        float currentScore = 0;
        if (assessNum == 1)
        {
            float scorePerGroup = assess1 / 4f;
            print(scorePerGroup);
            if (scorePerGroup > totalError)
            {
                currentScore = scorePerGroup - totalError;
                print(currentScore);
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

    private void AssessResult()
    {
        //StartCoroutine(UpdateCurrentScore());
        if (score < 50)
        {
            result[4].SetActive(true);
        }
        else
        {
            if (score >= 50 && score < 75)
            {
                result[1].SetActive(true);
                //delaytime = 4;
            }
            else if (score >= 75 && score < 100)
            {
                //delaytime = 4;
                result[0].SetActive(true);
                result[2].SetActive(true);
            }
            else
            {
                //delaytime += 8;
                result[0].SetActive(true);
                result[3].SetActive(true);
            }
            //StartCoroutine(GoToMap());
        }
    }
}
