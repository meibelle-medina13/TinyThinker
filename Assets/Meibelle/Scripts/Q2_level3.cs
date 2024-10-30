using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class Q2_level3 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] scenes = new GameObject[13];

    [SerializeField]
    private GameObject[] assessment1 = new GameObject[3];
    [SerializeField] 
    private GameObject[] assessment2 = new GameObject[4];
    [SerializeField] 
    private GameObject[] assessment3 = new GameObject[3];

    [SerializeField]
    private Image progressBar;
    [SerializeField]
    private GameObject[] result = new GameObject[5];

    int counter;
    int assess1 = 50;
    int assess2 = 25;
    int assess3 = 25;
    float score;
    int error = 0;

    private void Start()
    {
        checkAssessment1();
        checkAssessment2();
        checkAssessment3();
    }

    public void onContinue()
    {
        if (counter <= 11)
        {
            scenes[counter].SetActive(false);
            counter++;
            scenes[counter].SetActive(true);
        }
    }

    private void checkAssessment1()
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
            moveProgress(prev, assess1, error, assessNum);
        }
        else if (name == "right-arrow" && gameObject.name == "group2")
        {
            assessment1[0].SetActive(false);
            assessment2[0].SetActive(true);
            moveProgress(prev, assess1, error, assessNum);
        }
        else if (name == "triangle" && gameObject.name == "Assessment2")
        {
            assessment2[0].SetActive(false);
            assessment3[0].SetActive(true);
            moveProgress(prev, assess2, error, assessNum);
        }
        else if(name == "good" && gameObject.name == "group1")
        {
            assessment3[1].SetActive(false);
            assessment3[2].SetActive(true);
            moveProgress(prev, assess3, error, assessNum);
        }
        else if(name == "good" && gameObject.name == "group2")
        {
            scenes[11].SetActive(false);
            scenes[12].SetActive(true);
            moveProgress(prev, assess3, error, assessNum);
            assessResult();
        }
        else
        {
            if(assessNum == 1)
            {
                error += 12;
            }
            else if(assessNum == 2)
            {
                error += 8;
            }
            else if(assessNum == 3)
            {
                error += 6;
            }
        }
    }

    private void checkAssessment2()
    {
        for (int i = 1; i <= 3; i++)
        {
            Button option = assessment2[i].GetComponentInChildren<Button>();
            option.onClick.AddListener(() => validateAssessmentAns(option.name, assessment2[0], 2));
        }
    }

    private void checkAssessment3()
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

    private void moveProgress(float prev, int current, int totalError, int assessNum)
    {
        float currentScore = 0;
        if (assessNum == 1)
        {
            float scorePerGroup = 50f / 2f;
            if (scorePerGroup > totalError)
            {
                currentScore = scorePerGroup - totalError;
            }
            else
            {
                currentScore = 0;
            }
            error = 0;
        }
        else if (assessNum == 2)
        {
            float scorePerGroup = 25f;
            if (scorePerGroup > totalError)
            {
                currentScore = scorePerGroup - totalError;
            }
            else
            {
                currentScore = 0;
            }
            error = 0;
        }
        else if (assessNum == 3)
        {
            float scorePerGroup = 25f / 2f;
            if (scorePerGroup > totalError)
            {
                currentScore = scorePerGroup - totalError;
            }
            else
            {
                currentScore = 0;
            }
            error = 0;
        }
        score += currentScore;
        progressBar.fillAmount = score / 100;
    }

    private void assessResult()
    {
        if (score < 50)
        {
            Debug.Log("Failed!");
        }
        else if (score >= 50 && score < 75)
        {
            result[1].SetActive(true);
        }
        else if (score >= 75 && score < 100)
        {
            result[0].SetActive(true);
            result[2].SetActive(true);
        }
        else
        {
            result[0].SetActive(true);
            result[3].SetActive(true);
        }
    }
}
