using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TracingAndPreTestActivity : MonoBehaviour
{
    // TracingActivity variables
    public GameObject scene;
    public GameObject pencil;
    public GameObject PencilMask;
    public GameObject mycollider;
    private Vector3 pencilState;
    private Vector3 pencilRaise = new Vector3(105, 120, 0);
    private Vector3 pencilWrite = new Vector3(85, 100, 0);
    public Button NextButton;
    //public int targetScene = 0;

    private HashSet<string> tracedPoints = new HashSet<string>();
    private int score = 0;
    public int totalTracingPoints = 0;
    private bool isMousePressed = false;

    //public AudioSource correctSound;


    // Pre_Test variables
    public List<GameObject> preTest_scenes;
    public int pretest_counter = 0;
    public Button next;
    public int PreTest_Score;

    void Start()
    {
        
    }

    void Update()
    {
        // Handling tracing activity
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
        if (other.CompareTag("Tracing Point1") && !tracedPoints.Contains(other.gameObject.name))
        {
            tracedPoints.Add(other.gameObject.name);
            //UpdateScore();
            score++;
            //CheckCompletion();
            Debug.Log("Grade: " + score);
        }
    }

    //void CheckCompletion()
    //{
    //    if (targetScene == 1)
    //    {
    //        if (tracedPoints.Count >= totalTracingPoints)
    //        {
    //            //correctSound.Play();
    //            NextButton.gameObject.SetActive(true);
    //        }
    //    }
    //}
    //void UpdateScore()
    //{
    //    score++;
    //    //scoreText.text = "Score: " + score;
    //}

    public void UpdateGrade()
    {
        float percentage = (float)score / totalTracingPoints * 100;
        //string grade = GetGrade(percentage);
        GetGrade(percentage);
        UpdateScene();
        //gradeText.text = "Grade: " + grade;
        //Debug.Log("Grade: " + grade);

        //if (grade == "A")
        //{
        //    PreTest_Score += 4;
        //}
        //else if (grade == "B")
        //{
        //    PreTest_Score += 3;
        //}
        //else if (grade == "C")
        //{
        //    PreTest_Score += 2;
        //}
        //else if (grade == "D")
        //{
        //    PreTest_Score += 1;
        //}
    }

    //string GetGrade(float percentage)
    void GetGrade(float percentage)
    {
        if (percentage >= 90)
        {
            PreTest_Score += 4;
        }
        else if (percentage >= 80)
        {
            PreTest_Score += 3;
        }
        else if (percentage >= 70)
        {
            PreTest_Score += 2;
        }
        else if (percentage >= 60)
        {
            PreTest_Score += 1;
        }
        //if (percentage >= 90) return "A";
        //if (percentage >= 80) return "B";
        //if (percentage >= 70) return "C";
        //if (percentage >= 60) return "D";
        //return "F";
    }


    // Pre_Test methods
    public void Onclick()
    {
        // Add functionality as needed
    }

    public void UpdateScene()
    {
    
        preTest_scenes[pretest_counter].SetActive(false);
        pretest_counter++;
        preTest_scenes[pretest_counter].SetActive(true);

        // Update the next button state
        if (pretest_counter == 1)
        {
            next.gameObject.SetActive(true);
        }
        
    }
}
