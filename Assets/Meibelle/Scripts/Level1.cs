using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level1 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] scenes = new GameObject[8];

    [SerializeField]
    private Button[] exercise1 = new Button[3];
    [SerializeField]
    private GameObject cake;
    [SerializeField]
    private GameObject[] candles = new GameObject[4];
    Vector3[] candlesInitialPos = new Vector3[4];
    [SerializeField]
    private GameObject confetti;
    [SerializeField]
    private GameObject[] exercise2 = new GameObject[2];

    [SerializeField]
    private GameObject[] assessmentConfetti = new GameObject[3];
    [SerializeField]
    private GameObject[] assessment = new GameObject[4];
    [SerializeField]
    private Button[] assessment1button = new Button[3];
    [SerializeField]
    private Button[] assessment2button = new Button[3];
    [SerializeField]
    private Button[] assessment3button = new Button[3];
    [SerializeField]
    private TextMeshProUGUI[] assessment1text = new TextMeshProUGUI[3];

    [SerializeField]
    private Image progressBar;
    [SerializeField]
    private GameObject[] result = new GameObject[5];

    int counter = 0;
    int assess1 = 50;
    int assess2 = 25;
    int assess3 = 25;
    int score;

    public void Start()
    {
        exercise1[0].onClick.AddListener(wrongAns);
        exercise1[1].onClick.AddListener(() => correctAns(confetti));
        exercise1[2].onClick.AddListener(wrongAns);
        shuffleLetterChoices(0);

        foreach (Button button in assessment2button)
        {
            string text = button.name;
            button.onClick.AddListener(() => CheckAssessment2(text));
            Debug.Log("inloop" + text);
        }

        foreach (Button button in assessment3button)
        {
            string text = button.name;
            button.onClick.AddListener(() => CheckAssessment3(text));
            Debug.Log("inloop" + text);
        }

        for(int i=0; i < 4; i++)
        {
            candlesInitialPos[i] = candles[i].transform.position;
        }
    }

    public void onContinue()
    {

        if (counter <= 7)
        {
            scenes[counter].SetActive(false);
            counter++;
            scenes[counter].SetActive(true);
        }
    }

    private void correctAns(GameObject objName)
    {
        objName.SetActive(true);
    }

    private void wrongAns()
    {
        Debug.Log("Wrong Answer!");
    }

    private void shuffleLetterChoices(int index)
    {
        int current_index = UnityEngine.Random.Range(0, 3);
        string letter = assessment1text[index].text;
        assessment1button[current_index].GetComponentInChildren<TMP_Text>().text = letter;

        for (int j = 0; j < 3; j++)
        {
            if (j != current_index)
            {
                string randomLetter = chooseRandomLetter();
                assessment1button[j].GetComponentInChildren<TMP_Text>().text = randomLetter;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            string text = assessment1button[i].GetComponentInChildren<TMP_Text>().text;
            assessment1button[i].onClick.RemoveAllListeners();
            assessment1button[i].onClick.AddListener(() => CheckAssessment1(index, text));
            Debug.Log("inloop" + text);
        }

        //Debug.Log(current_index);
        //Debug.Log(letter);
    }

    private string chooseRandomLetter()
    {
        string letters = "B,C,D,E,F,G,H,I,J,K,L,M,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        string[] letterArray = letters.Split(',');

        return letterArray[UnityEngine.Random.Range(0, 24)];
    }

    private void CheckAssessment1(int i, string word)
    {
        if (assessment1text[i].color != Color.black && assessment1text[i].text == word)
        {
            Debug.Log("checkassessloop" + assessment1text[i].text + " " + word);
            assessment1text[i].color = Color.black;
            //shuffleLetterChoices(i + 1);
            if (i < 2)
            {
                shuffleLetterChoices(i + 1);
            }
            else
            {
                if (assess1 > 0)
                {
                    score += assess1;
                }
                else
                {
                    score += 0;
                }
                moveProgress(0, score);
                assessmentConfetti[0].SetActive(true);
                StartCoroutine(delayNextAssessment(0, 1));
            }
        }
        else
        {
            assess1 -= 2;
        }
    }

    private void CheckAssessment2(string word)
    {
        if (word == "nametag")
        {
            if (assess2 < 0)
            {
                assess2 = 0;
            }

            int prev = score;
            score += assess2;
            
            moveProgress(prev, score);
            correctAns(assessmentConfetti[1]);
            StartCoroutine(delayNextAssessment(1, 2));
        }
        else
        {
            assess2 -= 5;
            wrongAns();
        }
    }

    private void CheckAssessment3(string word)
    {
        if (word == "four")
        {
            if (assess3 < 0)
            {
                assess3 = 0;
            }
            int prev = score;
            score += assess3;

            moveProgress(prev, score);
            correctAns(assessmentConfetti[2]);
            StartCoroutine(delayNextAssessment(2, 3));
        }
        else
        {
            assess3 -= 5;
            wrongAns();
        }
    }

    IEnumerator delayNextAssessment(int current, int next)
    {
        yield return new WaitForSeconds(4);
        assessment[current].SetActive(false);
        
        if (next == 3)
        {
            assessment[next].SetActive(true);
            assessResult();
        } else
        {
            assessment[next].SetActive(true);
        }
    }

    public void DragCandle1()
    {
        candles[0].transform.position = Input.mousePosition;
        Debug.Log(candles[0].transform.position);
    }

    public void DragCandle2()
    {
        candles[1].transform.position = Input.mousePosition;
    }

    public void DragCandle3()
    {
        candles[2].transform.position = Input.mousePosition;
    }

    public void DragCandle4()
    {
        candles[3].transform.position = Input.mousePosition;
    }

    public void DropCandle1()
    {
        float Distance = Vector3.Distance(candles[0].transform.position, cake.transform.position);
        if (Distance < 150)
        {
            candles[0].transform.position = new Vector3(460.97f, 398.80f, 0.00f);
            afterDragDrop();
        }
        else
        {
            candles[0].transform.position = candlesInitialPos[0];
        }
    }

    public void DropCandle2()
    {
        float Distance = Vector3.Distance(candles[1].transform.position, cake.transform.position);
        if (Distance < 150)
        {
            candles[1].transform.position = new Vector3(507.46f, 398.80f, 0.00f);
            afterDragDrop();
        }
        else
        {
            candles[1].transform.position = candlesInitialPos[1];
        }
    }

    public void DropCandle3()
    {
        float Distance = Vector3.Distance(candles[2].transform.position, cake.transform.position);
        if (Distance < 150)
        {
            candles[2].transform.position = new Vector3(551.96f, 398.80f, 0.00f);
            afterDragDrop();
        }
        else
        {
            candles[2].transform.position = candlesInitialPos[2];
        }
    }

    public void DropCandle4()
    {
        float Distance = Vector3.Distance(candles[3].transform.position, cake.transform.position);
        if (Distance < 150)
        {
            candles[3].transform.position = new Vector3(604.71f, 398.80f, 0.00f);
            afterDragDrop();
        }
        else
        {
            candles[3].transform.position = candlesInitialPos[3];
        }
    }

    private void afterDragDrop()
    {
        int remaining = 4;
        for(int i = 0; i < candles.Length; i++)
        {
            if (candles[i].transform.position !=  candlesInitialPos[i])
            {
                remaining--;
            }
        }
        if (remaining == 0)
        {
            StartCoroutine(delayExercise2Result());
        }
    }

    IEnumerator delayExercise2Result()
    {
        yield return new WaitForSeconds(1.5f);
        exercise2[0].SetActive(false);
        exercise2[1].SetActive(true);
    }

    private void moveProgress(int starting, int score)
    {
        for (int i = starting; i <= score; i++)
        {
            float inDecimal = (float)i / 100;
            progressBar.fillAmount = inDecimal;
        }
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
