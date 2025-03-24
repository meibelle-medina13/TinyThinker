using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PreTest_PostTest2 : MonoBehaviour
{
    public int Level;
    private Audio_Manager test_audiomanager;
    public List<GameObject> Test_scenes;
    public List<GameObject> Test_timelines;
    public GameObject Title_timeline;
    public List<GameObject> Tracing_objects;
    public List<GameObject> Tracking_Test;
    public static int test_counter = 0;
    public static int Test_Score;
    //public int Level;
    public GameObject progress_display;
    public Image Fill;

    public GameObject scene;
    public GameObject pencil;
    public GameObject PencilMask;
    public GameObject mycollider;
    private Vector3 pencilState;
    private Vector3 pencilRaise = new Vector3(105, 120, 0);
    private Vector3 pencilWrite = new Vector3(85, 100, 0);

    private HashSet<string> tracedPoints = new HashSet<string>();
    private int score = 0;
    public int totalTracingPoints = 0;

    private AudioSource audioSource;
    //private static bool bgMusicPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        test_audiomanager = FindObjectOfType<Audio_Manager>();

        if (gameObject.name == "Pre-Test_SceneManager" || gameObject.name == "Post-Test_SceneManager")
        {
            PlayerPrefs.DeleteKey("CurrentPanel");
        }

        //if (!bgMusicPlayed)
        //{
        //    if (test_audiomanager != null)
        //    {
        //        test_audiomanager.scene_bgmusic(0.5f);
        //        bgMusicPlayed = true;
        //    }
        //}

    }

    void Update()
    {
        //Debug.Log(Level);

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
            if (CompareTag("pencil"))
            {
                mycollider.GetComponent<CircleCollider2D>().enabled = true;
                pencilState = pencilWrite;

                GameObject pencilMask = Instantiate(PencilMask, worldPosition, Quaternion.identity);
                pencilMask.transform.SetParent(scene.transform);
            }
        }

        else
        {
            if (CompareTag("pencil"))
            {
                mycollider.GetComponent<CircleCollider2D>().enabled = false;
                pencilState = pencilRaise;
            }

        }


        if (Input.GetMouseButtonUp(0))
        {
            if (CompareTag("pencil"))
            {
                mycollider.GetComponent<CircleCollider2D>().enabled = false;
                pencilState = pencilRaise;
            }
        }

        if (CompareTag("pencil"))
        {
            pencil.transform.position = worldPosition + pencilState;
            mycollider.transform.position = worldPosition;
        }

        int index = 0;

        if (gameObject.name == "Pre-Test_SceneManager" || gameObject.name == "Post-Test_SceneManager")
        {

            PlayableDirector playableDirector;

            if (PlayerPrefs.HasKey("CurrentPanel"))
            {
                index = PlayerPrefs.GetInt("CurrentPanel");
                Debug.Log("Panel" + PlayerPrefs.GetInt("CurrentPanel"));
                playableDirector = Test_timelines[index].GetComponent<PlayableDirector>();
            }
            else
            {
                playableDirector = Title_timeline.GetComponent<PlayableDirector>();
            }

            if (PlayerPrefs.GetString("Paused") == "True")
            {
                playableDirector.Pause();
                if (Test_scenes[index].name == "Pre-Test 1" || Test_scenes[index].name == "Post-Test 1")
                {
                    Tracing_objects[0].SetActive(false);
                }
                else if (Test_scenes[index].name == "Pre-Test 3" || Test_scenes[index].name == "Post-Test 3")
                {
                    Tracing_objects[1].SetActive(false);
                }
                else if (Test_scenes[index].name == "Pre-Test 5" || Test_scenes[index].name == "Post-Test 5")
                {
                    Tracing_objects[2].SetActive(false);
                }
                else if (Test_scenes[index].name == "Pre-Test 7" || Test_scenes[index].name == "Post-Test 7")
                {
                    Tracing_objects[3].SetActive(false);
                }
                else if (Test_scenes[index].name == "Pre-Test 9" || Test_scenes[index].name == "Post-Test 9")
                {
                    Tracing_objects[4].SetActive(false);
                }
                else if (Test_scenes[index].name == "Pre-Test 11" || Test_scenes[index].name == "Post-Test 11")
                {
                    Tracing_objects[5].SetActive(false);
                }
                else if (Test_scenes[index].name == "Pre-Test 12" || Test_scenes[index].name == "Post-Test 12")
                {
                    Tracing_objects[6].SetActive(false);
                }
            }
            else if (PlayerPrefs.GetString("Paused") == "False")
            {
                playableDirector.Resume();
                if (Test_scenes[index].name == "Pre-Test 1" || Test_scenes[index].name == "Post-Test 1")
                {
                    Tracing_objects[0].SetActive(true);
                }
                else if (Test_scenes[index].name == "Pre-Test 3" || Test_scenes[index].name == "Post-Test 3")
                {
                    Tracing_objects[1].SetActive(true);
                }
                else if (Test_scenes[index].name == "Pre-Test 5" || Test_scenes[index].name == "Post-Test 5")
                {
                    Tracing_objects[2].SetActive(true);
                }
                else if (Test_scenes[index].name == "Pre-Test 7" || Test_scenes[index].name == "Post-Test 7")
                {
                    Tracing_objects[3].SetActive(true);
                }
                else if (Test_scenes[index].name == "Pre-Test 9" || Test_scenes[index].name == "Post-Test 9")
                {
                    Tracing_objects[4].SetActive(true);
                }
                else if (Test_scenes[index].name == "Pre-Test 11" || Test_scenes[index].name == "Post-Test 11")
                {
                    Tracing_objects[5].SetActive(true);
                }
                else if (Test_scenes[index].name == "Pre-Test 12" || Test_scenes[index].name == "Post-Test 12")
                {
                    Tracing_objects[6].SetActive(true);
                }
            }
        }

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
        DelayUpdate();
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
            Test_Score++;
        }
        Debug.Log("Score: " + Test_Score);
    }

    void IncrementFillAmount()
    {
        if (Test_scenes.Count == 10)
        {
            Fill.fillAmount = Mathf.Clamp01(Fill.fillAmount + 0.1428571428571429f);
        }

        else if (Test_scenes.Count == 14)
        {
            Fill.fillAmount = Mathf.Clamp01(Fill.fillAmount + 0.0909090909090909f);
        }

        else if (Test_scenes.Count == 19)
        {
            Fill.fillAmount = Mathf.Clamp01(Fill.fillAmount + 0.0625f);
        }

    }

    public void TitleUpdateScene()
    {
        this.gameObject.SetActive(false);
        Test_scenes[test_counter].SetActive(true);
        PlayerPrefs.SetInt("CurrentPanel", test_counter);
    }

    public void DelayUpdate()
    {
        Invoke("UpdateScene", 1f);
    }

    public void UpdateScene()
    {
        Test_scenes[test_counter].SetActive(false);
        test_counter++;
        Test_scenes[test_counter].SetActive(true);
        PlayerPrefs.SetInt("CurrentPanel", test_counter);

        if (test_counter < (Test_scenes.Count - 1))
        {
            Tracking_Test[test_counter].SetActive(false);
        }

        if (test_counter > 1)
        {
            IncrementFillAmount();
        }

        PlayerPrefs.SetInt("Test Score", Test_Score);
        PlayerPrefs.SetInt("Current Test", Level);
        Debug.Log("Score: " + Test_Score);
    }

    public void Add_Point()
    {
        Test_Score++;
        DelayUpdate();
    }


    //// ------------------------------------------------------------------- //
    //int userID;
    //public void GetTotalScore()
    //{
    //    Debug.Log("FInal:" + Test_Score);
    //    userID = PlayerPrefs.GetInt("Current_user");
    //    StartCoroutine(GoToMap());
    //}

    //// -------------------------------------------------------------------- //

    //int delaytime;
    //IEnumerator GoToMap()
    //{
    //    yield return new WaitForSeconds(delaytime);
    //    StartCoroutine(UpdateCurrentLevel());
    //}

    //IEnumerator UpdateCurrentLevel()
    //{
    //    int current_level = 1;
    //    byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"current_level\": " + current_level + "}");

    //    if (Test_Score >= 50)
    //    {
    //        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users", rawData))
    //        {
    //            www.method = "PUT";
    //            www.SetRequestHeader("Content-Type", "application/json");
    //            yield return www.SendWebRequest();

    //            if (www.result != UnityWebRequest.Result.Success)
    //            {
    //                Debug.LogError(www.error);
    //            }
    //            else
    //            {
    //                PlayerPrefs.SetInt("Current_level", current_level);
    //                Debug.Log("Received: " + www.downloadHandler.text);
    //                UnityEngine.SceneManagement.SceneManager.LoadScene(7);
    //            }
    //        }
    //    }

    //}

    //IEnumerator UpdateCurrentScore()
    //{
    //    byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"theme_num\": 1, \"level_num\": 1, \"score\": " + score + "}");

    //    using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/scores", rawData))
    //    {
    //        www.method = "PUT";
    //        www.SetRequestHeader("Content-Type", "application/json");
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.LogError(www.error);
    //        }
    //        else
    //        {
    //            Debug.Log("Received: " + www.downloadHandler.text);
    //        }
    //    }
    //}
}

