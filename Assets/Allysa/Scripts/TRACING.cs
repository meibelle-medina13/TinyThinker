using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure to include TextMeshPro namespace

public class TracingActivity : MonoBehaviour
{
    public GameObject scene;
    public GameObject pencil;
    public GameObject PencilMask;
    public GameObject mycollider;
    private Vector3 pencilState;
    private Vector3 pencilRaise = new Vector3(105, 120, 0);
    private Vector3 pencilWrite = new Vector3(85, 100, 0);
    public Button NextButton;
    public int targetScene = 0;

    //public TextMeshProUGUI gradeText;
    //public TextMeshProUGUI scoreText;

    private HashSet<string> tracedPoints = new HashSet<string>();
    private int score = 0;
    public int totalTracingPoints = 0; 
    private bool isMousePressed = false;

    public AudioSource correctSound;

    private Theme2Level4_SceneManager scenemanagerL2_4;
    void Start()
    {
        scenemanagerL2_4 = FindObjectOfType<Theme2Level4_SceneManager>();
        if (targetScene == 1)
        {
            NextButton.gameObject.SetActive(false);
        }
        else
        {
            NextButton.gameObject.SetActive(true);
        }

    }
    
    void Update()
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane + 1;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (Input.GetMouseButton(0))
        {
            mycollider.GetComponent<CircleCollider2D>().enabled = true;
            pencilState = pencilWrite;

            GameObject pencilMask = Instantiate(PencilMask, worldPosition, Quaternion.identity);
            pencilMask.transform.SetParent(scene.transform);
        }

        else
        {
            mycollider.GetComponent<CircleCollider2D>().enabled = false; 
            pencilState = pencilRaise; 
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMousePressed = false;
            mycollider.GetComponent<CircleCollider2D>().enabled = false;
            pencilState = pencilRaise;  
        }

        pencil.transform.position = worldPosition + pencilState;
        mycollider.transform.position = worldPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tracing Point") && !tracedPoints.Contains(other.gameObject.name))
        {
            tracedPoints.Add(other.gameObject.name);
            UpdateScore();
            CheckCompletion();
        }
    }

    void CheckCompletion() 
    {
        if (targetScene == 1)
        {
            if (tracedPoints.Count >= totalTracingPoints)
            {
                correctSound.Play();
                NextButton.gameObject.SetActive(true);
            }
        }
    }
    void UpdateScore()
    {
        score++;
        //scoreText.text = "Score: " + score;
    }

    public void UpdateGrade()
    {
        float percentage = (float)score / totalTracingPoints * 100;
        string grade = GetGrade(percentage);
        //gradeText.text = "Grade: " + grade;
        Debug.Log("Grade: " + grade);

        scenemanagerL2_4.whenButtonClicked();

        if (grade == "A")
        {
            scenemanagerL2_4.IncrementFillAmount(0.2857142857142858f);
        }
        else if (grade == "B")
        {
            scenemanagerL2_4.IncrementFillAmount(0.2142857142857143f);
        }
        else if (grade == "C")
        {
            scenemanagerL2_4.IncrementFillAmount(0.1428571428571428f);
        }
        else if (grade == "D")
        {
            scenemanagerL2_4.IncrementFillAmount(0.0714285714285715f);
        }
    }

    string GetGrade(float percentage)
    {
        if (percentage >= 90) return "A";
        if (percentage >= 80) return "B";
        if (percentage >= 70) return "C";
        if (percentage >= 60) return "D";
        return "F";
    }
}

