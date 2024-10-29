using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Level2 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] preview = new GameObject[2];
    [SerializeField]
    private GameObject[] scenes = new GameObject[10];

    [SerializeField]
    private Button[] exercise1 = new Button[8];
    [SerializeField]
    private Button[] exercise2 = new Button[2];

    [SerializeField]
    private GameObject[] assessment1 = new GameObject[3];
    [SerializeField]
    private GameObject[] assessment2 = new GameObject[3];
    Vector3[] keyInitialPos = new Vector3[3];
    [SerializeField]
    private GameObject treasure_box, sticker_cake;
    [SerializeField]
    private GameObject[] ballons = new GameObject[2];
    [SerializeField]
    private GameObject[] assessment3 = new GameObject[2];

    [SerializeField]
    private GameObject[] assessment = new GameObject[3];

    [SerializeField]
    private Image progressBar;

    //[SerializeField]
    //private GameObject[] result = new GameObject[5];
    [SerializeField]
    private GameObject[] result = new GameObject[5];

    int counter;
    int score;
    int assess1 = 50;
    int assess2 = 25;
    int assess3 = 25;

    private void Start()
    {
        foreach (Button letter in exercise1) 
        {
            string text = letter.name;
            letter.onClick.AddListener(() => CheckExercise1(letter, text));
        }

        foreach (Button number in exercise2)
        {
            string text = number.name;
            number.onClick.AddListener(() => CheckExercise2(text));
        }

        Button treasure = assessment1[0].GetComponent<Button>();
        treasure.onClick.AddListener(() => getTreasureBox());

        for (int i = 0; i < 3; i++)
        {
            keyInitialPos[i] = assessment2[i].transform.position;
        }

        foreach (GameObject balloon in ballons)
        {
            Button button = balloon.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => CheckAssessment3(balloon));
        }

    }

    public void managePreview()
    {
        preview[0].SetActive(false);
        preview[1].SetActive(true);
    }
    public void onContinue()
    {

        if (counter <= 9)
        {
            scenes[counter].SetActive(false);
            counter++;
            scenes[counter].SetActive(true);
        }
    }

    private void CheckExercise1(Button button, string text)
    {
        int correct = 0;
        ColorBlock buttonColor = button.colors;

        if (text == "Aa")
        {
            buttonColor.normalColor = Color.green;
            buttonColor.selectedColor = Color.green;
            //correct++;
        }
        else
        {
            buttonColor.selectedColor = Color.red;
        }

        button.colors = buttonColor;

        for (int i = 0; i < 7; i++)
        {
            ColorBlock bc = exercise1[i].colors;
            if (bc.normalColor == Color.green)
            {
                correct++;
            }
        }

        Debug.Log(correct);

        if (correct == 3)
        {
            Button next_button = exercise1[7];
            next_button.gameObject.SetActive(true);
        }
    }

    private void CheckExercise2(string text)
    {
        if (text == "1")
        {
            onContinue();
        }
    }

    public void getTreasureBox()
    {
        score += assess1;
        moveProgress(0, score);
        assessment1[1].SetActive(false);
        assessment1[2].SetActive(true);
    }

    public void dragKeyH()
    {
        assessment2[0].transform.position = Input.mousePosition;
    }

    public void dragKeyE()
    {
        assessment2[1].transform.position = Input.mousePosition;
    }

    public void dragKeyA()
    {
        assessment2[2].transform.position = Input.mousePosition;
    }

    public void dropKeyH()
    {
        assessment2[0].transform.position = keyInitialPos[0];
        assess2 -= 5;
    }

    public void dropKeyE()
    {
        assessment2[1].transform.position = keyInitialPos[1];
        assess2 -= 5;
    }

    public void dropKeyA()
    {
        float distance = Vector3.Distance(assessment2[2].transform.position, treasure_box.transform.position);
        if (distance < 200)
        {
            assessment2[2].transform.Rotate(0, 0, -96);
            assessment2[2].transform.position = new Vector3 (850, 197, 0);
            if (assess2 > 0)
            {
                score += assess2;
            }
            else
            {
                score += 0;
            }
            moveProgress(50, score);

            StartCoroutine(delayAssessment3());
        }
    }

    IEnumerator delayAssessment3()
    {
        yield return new WaitForSeconds(1);
        assessment[1].SetActive(false);
        assessment[2].SetActive(true);
    }

    private void CheckAssessment3(GameObject balloon)
    {
        if (balloon.name == "1")
        {
            if (assess3 > 0)
            {
                score += assess3;
            }
            else
            {
                score += 0;
            }

            moveProgress(75, score);
            balloon.SetActive(false);
            sticker_cake.SetActive(true);
            StartCoroutine(delayAssessmentResult());
        }
        else
        {
            assess3 -= 10;
        }
    }

    IEnumerator delayAssessmentResult()
    {
        yield return new WaitForSeconds(1);
        assessment3[0].SetActive(false);
        assessment3[1].SetActive(true);
        StartCoroutine(showResult());
    }

    private void moveProgress(int starting, int score)
    {
        for(int i = starting; i <= score; i++)
        {
            float inDecimal = (float)i / 100;
            progressBar.fillAmount = inDecimal;
        }
    }

    IEnumerator showResult()
    {
        yield return new WaitForSeconds(2.5f);
        assessment3[1].SetActive(false);
        result[0].SetActive(true);
        assessResult();
    }

    private void assessResult()
    {
        if(score < 50)
        {
            Debug.Log("Failed!");
        }
        else if(score >= 50 && score < 75)
        {
            result[2].SetActive(true);
        }
        else if(score >= 75 && score < 100)
        {
            result[1].SetActive(true);
            result[3].SetActive(true);
        }
        else
        {
            result[1].SetActive(true);
            result[4].SetActive(true);
        }
    }
}
