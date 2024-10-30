using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Q2_level2 : MonoBehaviour
{

    [SerializeField]
    private GameObject[] scenes = new GameObject[8];

    [SerializeField]
    private GameObject[] assessment = new GameObject[4];

    [SerializeField]
    private GameObject[] assessment1 = new GameObject[3];

    [SerializeField]
    private GameObject[] assessment2 = new GameObject[3];

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

    [SerializeField]
    private Image progressBar;
    [SerializeField]
    private GameObject[] result = new GameObject[5];

    int objectCounter;
    int assess1 = 50;
    int assess2 = 25;
    int assess3 = 25;
    float score;
    int error1 = 0;
    int error2 = 0;

    Vector3[] colorsInitialPos = new Vector3[2];
    private void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            int index = i;
            Button next = scenes[i].GetComponentInChildren<Button>();
            next.onClick.AddListener(() => onContinue(index));
        }
        checkAssessment1();
        checkAssessment2();
        checkAssessment3();
    }

    private void onContinue(int index)
    {
        print(index);
        scenes[index].SetActive(false);
        scenes[index+1].SetActive(true);
    }

    private void checkAssessment1()
    {
        foreach (GameObject group in assessment1)
        {
            foreach (Button item in group.GetComponentsInChildren<Button>())
            {
                item.onClick.AddListener(() => validateAssessmentAns(1, item.name, group, 'o'));
            }
        }
    }

    private void checkAssessment2()
    {
        foreach (GameObject group in assessment2)
        {
            foreach (Button item in group.GetComponentsInChildren<Button>())
            {
                item.onClick.AddListener(() => validateAssessmentAns(2, item.name, group, 'u'));
            }
        }
    }

    private void validateAssessmentAns(int assessNum, string name, GameObject group, char criteria)
    {
        float prev = 0;

        if ((group.name == "group1") && name[0] == criteria)
        {
            prev = score;
            group.SetActive(false);
            if (assessNum == 1)
            {
                assessment1[1].SetActive(true);
                moveProgress(prev, assess1, error1, 1);
            }
            else
            {
                assessment2[1].SetActive(true);
                moveProgress(prev, assess2, error2, 2);
            }
        }
        else if ((group.name == "group2") && name[0] == criteria)
        {
            prev = score;
            group.SetActive(false);
            if (assessNum == 1)
            {
                assessment1[2].SetActive(true);
                moveProgress(prev, assess1, error1, 1);
            }
            else
            {
                assessment2[2].SetActive(true);
                moveProgress(prev, assess2, error2, 2);
            }
        }
        else if ((group.name == "group3") && name[0] == criteria)
        {
            prev = score;
            if (assessNum == 1)
            {
                assessment[0].SetActive(false);
                assessment[1].SetActive(true);
                moveProgress(prev, assess1, error1, 1);
            }
            else
            {
                assessment[1].SetActive(false);
                assessment[2].SetActive(true);
                moveProgress(prev, assess2, error2, 2);
            }
        }
        else
        {
            if (assessNum == 1)
            {
                error1 += 8;
            }
            else
            {
                error2 += 4;
            }
        }
    }

    private void checkAssessment3()
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
        float prev = 0;
        float Distance1 = Vector3.Distance(redObjects[0].transform.position, colors[0].transform.position);
        float Distance2 = Vector3.Distance(redObjects[1].transform.position, colors[0].transform.position);
        float Distance3 = Vector3.Distance(redObjects[2].transform.position, colors[0].transform.position);

        if (Distance1 < 50)
        {
            prev = score;
            redObjects[0].sprite = redSprites[0];
            objectCounter++;
            moveProgress(prev, assess3, error1, 3);
            colors[0].transform.position = colorsInitialPos[0];
        }
        else if (Distance2 < 50)
        {
            prev = score;
            redObjects[1].sprite = redSprites[1];
            objectCounter++;
            colors[0].transform.position = colorsInitialPos[0];
            moveProgress(prev, assess3, error1, 3);
        }
        else if (Distance3 < 50)
        {
            prev = score;
            redObjects[2].sprite = redSprites[2];
            objectCounter++;
            colors[0].transform.position = colorsInitialPos[0];
            moveProgress(prev, assess3, error1, 3);
        }
        else
        {
            assess3 -= 4;
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
        float prev = 0;
        float Distance1 = Vector3.Distance(blueObjects[0].transform.position, colors[1].transform.position);
        float Distance2 = Vector3.Distance(blueObjects[1].transform.position, colors[1].transform.position);
        float Distance3 = Vector3.Distance(blueObjects[2].transform.position, colors[1].transform.position);

        if (Distance1 < 50)
        {
            prev = score;
            blueObjects[0].sprite = blueSprites[0];
            objectCounter++;
            colors[1].transform.position = colorsInitialPos[1];
            moveProgress(prev, assess3, error1, 3);
        }
        else if (Distance2 < 50)
        {
            prev = score;
            blueObjects[1].sprite = blueSprites[1];
            objectCounter++;
            colors[1].transform.position = colorsInitialPos[1];
            moveProgress(prev, assess3, error1, 3);
        }
        else if (Distance3 < 50)
        {
            prev = score;
            blueObjects[2].sprite = blueSprites[2];
            objectCounter++;
            colors[1].transform.position = colorsInitialPos[1];
            moveProgress(prev, assess3, error1, 3);
        }
        else
        {
            assess3 -= 4;
            colors[1].transform.position = colorsInitialPos[1];
        }

        if (objectCounter == 6)
        {
            scenes[7].SetActive(false);
            assessment[3].SetActive(true);
            assessResult();
        }
    }

    private void moveProgress(float prev, int current, int totalError, int assessNum)
    {
        float currentScore = 0;
        if (assessNum == 1)
        {
            float scorePerGroup = 50f / 3f;
            if (scorePerGroup > totalError)
            {
                currentScore = scorePerGroup - totalError;
            }
            else
            {
                currentScore = 0;
            }
            error1 = 0;
        }
        else if (assessNum == 2)
        {
            float scorePerGroup = 25f / 3f;
            if (scorePerGroup > totalError)
            {
                currentScore = scorePerGroup - totalError;
            }
            else
            {
                currentScore = 0;
            }
            error2 = 0;
        }
        else if (assessNum == 3)
        {
            if (current > 0)
            {
                float scorePerGroup = current / 6f;
                currentScore = scorePerGroup;
            }
            else
            {
                currentScore = 0;
            }
        }
        score += currentScore;
        progressBar.fillAmount = score / 100;
    }

    private void assessResult()
    {
        if (score < 50)
        {
            result[4].SetActive(true);
        }
        else if (score >= 50 && score < 75)
        {
            result[1].SetActive(true);
        }
        else if (score >= 75 && score < 99)
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
