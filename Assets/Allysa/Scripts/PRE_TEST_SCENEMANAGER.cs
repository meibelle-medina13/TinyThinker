using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class PreTest_PostTest : MonoBehaviour
{
    public List<GameObject> Test_scenes;
    public List<GameObject> Tracking_Test;
    public static int test_counter = 0;
    public static int Test_Score;
    public GameObject progress_display;
    public UnityEngine.UI.Image Fill;
    public List<TextMeshProUGUI> textWithOutline_PreTest_PostTest;

    public GameObject scene;
    public GameObject pencil;
    public GameObject PencilMask;
    public GameObject mycollider;
    private Vector3 pencilState;
    private Vector3 pencilRaise = new Vector3(105, 120, 0);
    private Vector3 pencilWrite = new Vector3(85, 100, 0);
    public Button next;

    private HashSet<string> tracedPoints = new HashSet<string>();
    private int score = 0;
    public int totalTracingPoints = 0;

    void Start()
    {
        foreach (TextMeshProUGUI text in textWithOutline_PreTest_PostTest)
        {
            text.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.4f);
            text.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.white);
        }
    }

    void Update()
    {
        if (test_counter > 0 && test_counter < (Test_scenes.Count - 1))
        {
            progress_display.SetActive(true);
        }
        else
        {
            progress_display.SetActive(false);
        }

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
            score++;
            Debug.Log("points: " + score);
        }
    }

    public void UpdateScore()
    {
        float percentage = (float)score / totalTracingPoints * 100;
        GetScore(percentage);
        UpdateScene();
        //Debug.Log("points: " + test_counter);
    }

    void GetScore(float percentage)
    {
        if (percentage >= 90)
        {
            Test_Score += 4;
        }
        else if (percentage >= 80)
        {
            Test_Score += 3;
        }
        else if (percentage >= 70)
        {
            Test_Score += 2;
        }
        else if (percentage >= 60)
        {
            Test_Score ++;
        }
        Debug.Log("Score: " + Test_Score);
    }

    void IncrementFillAmount()
    {
        if (Test_scenes.Count == 14)
        {
            Fill.fillAmount = Mathf.Clamp01(Fill.fillAmount + 0.0909090909090909f);
        }

        else if (Test_scenes.Count == 19)
        {
            Fill.fillAmount = Mathf.Clamp01(Fill.fillAmount + 0.0625f);
        }
    }

    public void UpdateScene()
    {
        Test_scenes[test_counter].SetActive(false);
        test_counter++;
        Test_scenes[test_counter].SetActive(true);

        if (test_counter < (Test_scenes.Count - 1))
        {
            Tracking_Test[test_counter].SetActive(false);
        }

        if (test_counter > 1)
        {
            IncrementFillAmount();
        }

        Debug.Log("Score: " + Test_Score);
    }

    public void Add_Point()
    { 
        Test_Score ++;
        UpdateScene();
    }
}
