using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure to include TextMeshPro namespace

public class Quarter2_Level4 : MonoBehaviour
{
    private static int Scene_counter = 0;
    private static bool bgMusicPlayed = false;
    private Vector3 pencilWrite = new Vector3(85, 100, 0);
    private Vector3 pencilState;
    private Vector3 pencilRaise = new Vector3(105, 120, 0);
    private HashSet<string> tracedPoints = new HashSet<string>();
    private int score = 0;
    //private static int instruction_count = 1;
    private Audio_Manager audioManager3;

    //BUTTON
    private int Wrong_Click = 0;

    public List<GameObject> scenes;
    public List<GameObject> Gameobjects;
    public List<TextMeshProUGUI> text;
    public List<Button> clickablebuttons;
    public List<GameObject> star_display;

    public Image fill_bar;
    public Button NextScene_Button;
    public int totalTracingPoints = 0;
    void Start()
    {
        //scenemanagerL2_4 = FindObjectOfType<Theme2Level4_SceneManager>();

        if (Gameobjects[0].name == "Scene2")
        {
            clickablebuttons[0].gameObject.SetActive(false);
        }
        else
        {
            clickablebuttons[0].gameObject.SetActive(true);
        }

        audioManager3 = FindObjectOfType<Audio_Manager>();

        if (!bgMusicPlayed)
        {
            if (audioManager3 != null)
            {
                audioManager3.scene_bgmusic();
                bgMusicPlayed = true;
                audioManager3.audioSourceBG1.volume = 0.39f;

            }
        }

        if (text[0] != null)
        {
            text[0].fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.3f);
            text[0].fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        }

    }

    void Update()
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane + 1;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (Input.GetMouseButton(0))
        {
            Gameobjects[3].GetComponent<CircleCollider2D>().enabled = true;
            pencilState = pencilWrite;

            GameObject pencilMask = Instantiate(Gameobjects[2], worldPosition, Quaternion.identity);
            pencilMask.transform.SetParent(Gameobjects[0].transform);
        }

        else
        {
            Gameobjects[3].GetComponent<CircleCollider2D>().enabled = false;
            pencilState = pencilRaise;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Gameobjects[3].GetComponent<CircleCollider2D>().enabled = false;
            pencilState = pencilRaise;
        }

        Gameobjects[1].transform.position = worldPosition + pencilState;
        Gameobjects[3].transform.position = worldPosition;
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
        if (Gameobjects[0].name == "Scene2")
        {
            if (tracedPoints.Count >= totalTracingPoints)
            {
                audioManager3.Correct();
                clickablebuttons[0].gameObject.SetActive(true);
            }
        }
    }
    void UpdateScore()
    {
        score++;
    }

    public void UpdateGrade()
    {
        float percentage = (float)score / totalTracingPoints * 100;
        string grade = GetGrade(percentage);
        //gradeText.text = "Grade: " + grade;
        Debug.Log("Grade: " + grade);

        //scenemanagerL2_4.whenButtonClicked();

        //if (grade == "A")
        //{
        //    scenemanagerL2_4.IncrementFillAmount(0.2857142857142858f);
        //}
        //else if (grade == "B")
        //{
        //    scenemanagerL2_4.IncrementFillAmount(0.2142857142857143f);
        //}
        //else if (grade == "C")
        //{
        //    scenemanagerL2_4.IncrementFillAmount(0.1428571428571428f);
        //}
        //else if (grade == "D")
        //{
        //    scenemanagerL2_4.IncrementFillAmount(0.0714285714285715f);
        //}
    }

    string GetGrade(float percentage)
    {
        if (percentage >= 90) return "A";
        if (percentage >= 80) return "B";
        if (percentage >= 70) return "C";
        if (percentage >= 60) return "D";
        return "F";
    }





    //////


    public void UpdateScene()
    {
        scenes[Scene_counter].SetActive(false);
        Scene_counter++;
        scenes[Scene_counter].SetActive(true);
    }
}

