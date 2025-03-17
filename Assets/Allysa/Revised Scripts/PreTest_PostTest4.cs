using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PreTest_PostTest4 : MonoBehaviour
{
    public int Level;
    private Audio_Manager test_audiomanager;
    public List<GameObject> Test_scenes;
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
    private Vector3 pencilRaise = new Vector3(105, 120, 0);
    private Vector3 pencilWrite = new Vector3(85, 100, 0);
    private Vector3 pencilState;
    private Vector3 offset;

    public List<Collider2D> setting_colliders;
    public List<GameObject> attachedobject;
    public List<Button> button;
    private HashSet<string> tracedPoints = new HashSet<string>();
    private int score = 0;
    public int totalTracingPoints = 0;

    private int object_counter = 0;
    private Transform draggable;
    private Vector3 draggableObjectState;
    private static bool increment = false;


    private AudioSource audioSource;
    private static bool bgMusicPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        test_audiomanager = FindObjectOfType<Audio_Manager>();

        if (!bgMusicPlayed)
        {
            if (test_audiomanager != null)
            {
                test_audiomanager.scene_bgmusic(0.5f);
                bgMusicPlayed = true;
            }
        }

    }
    void Update()
    {
        if (test_counter > 0 && test_counter < 13)
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

        if (Input.GetMouseButtonDown(0))
        {
            if (CompareTag("Draggable"))
            {
                increment = false;
                Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition);

                if (hitCollider != null && hitCollider.CompareTag("Draggable"))
                {
                    draggable = hitCollider.transform;
                    offset = draggable.position - worldPosition;
                }
            }
        }


        if (Input.GetMouseButton(0))
        {
            if (CompareTag("pencil"))
            {
                mycollider.GetComponent<CircleCollider2D>().enabled = true;
                pencilState = pencilWrite;

                GameObject pencilMask = Instantiate(PencilMask, worldPosition, Quaternion.identity);
                pencilMask.transform.SetParent(scene.transform);
            }

            if (CompareTag("Draggable"))
            {
                draggable.transform.position = worldPosition + offset;
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

            if (CompareTag("Draggable"))
            {
                draggable.position = worldPosition + draggableObjectState;
                test_audiomanager.Rotate();

                Collider2D[] Colliders = Physics2D.OverlapPointAll(draggable.transform.position);

                foreach (Collider2D area in Colliders)
                {
                    if (setting_colliders.Contains(area))
                    {
                        if (area.name == "setting 1" && draggable.name == "baka")
                        {
                            draggable.gameObject.SetActive(false);
                            attachedobject[6].SetActive(true);
                            CheckInactiveGameObjects();

                            if (!increment)
                            {
                                Test_Score++;
                                increment = true;
                            }
                        }

                        else if (area.name == "setting 1" && draggable.name == "baboy")
                        {

                            draggable.gameObject.SetActive(false);
                            attachedobject[5].SetActive(true);
                            CheckInactiveGameObjects();
                            break;
                        }

                        else if (area.name == "setting 1" && draggable.name == "palaka")
                        {
                            draggable.gameObject.SetActive(false);
                            attachedobject[4].SetActive(true);
                            CheckInactiveGameObjects();
                            break;
                        }

                        else if (area.name == "setting 1" && draggable.name == "teri")
                        {
                            draggable.gameObject.SetActive(false);
                            attachedobject[7].SetActive(true);
                            CheckInactiveGameObjects();
                            break;
                        }

                        else if (area.name == "setting 2" && draggable.name == "baboy")
                        {
                            draggable.gameObject.SetActive(false);
                            attachedobject[9].SetActive(true);
                            CheckInactiveGameObjects();

                            if (!increment)
                            {
                                Test_Score++;
                                increment = true;
                            }
                        }

                        else if (area.name == "setting 2" && draggable.name == "baka")
                        {
                            draggable.gameObject.SetActive(false);
                            attachedobject[10].SetActive(true);
                            CheckInactiveGameObjects();
                            break;
                        }

                        else if (area.name == "setting 2" && draggable.name == "palaka")
                        {
                            draggable.gameObject.SetActive(false);
                            attachedobject[8].SetActive(true);
                            CheckInactiveGameObjects();
                            break;
                        }

                        else if (area.name == "setting 2" && draggable.name == "teri")
                        {
                            draggable.gameObject.SetActive(false);
                            attachedobject[11].SetActive(true);
                            CheckInactiveGameObjects();
                            break;
                        }

                        else if (area.name == "setting 3" && draggable.name == "palaka")
                        {
                            draggable.gameObject.SetActive(false);
                            attachedobject[12].SetActive(true);
                            CheckInactiveGameObjects();

                            if (!increment)
                            {
                                Test_Score++;
                                increment = true;
                            }
                        }

                        else if (area.name == "setting 3" && draggable.name == "teri")
                        {
                            draggable.gameObject.SetActive(false);
                            attachedobject[15].SetActive(true);
                            CheckInactiveGameObjects();

                            if (!increment)
                            {
                                Test_Score++;
                                increment = true;
                            }

                            Debug.Log("score: " + Test_Score);
                            break;
                        }

                        else if (area.name == "setting 3" && draggable.name == "baka")
                        {
                            draggable.gameObject.SetActive(false);
                            attachedobject[17].SetActive(true);
                            CheckInactiveGameObjects();
                            break;
                        }

                        else if (area.name == "setting 3" && draggable.name == "baboy")
                        {
                            draggable.gameObject.SetActive(false);
                            attachedobject[16].SetActive(true);
                            CheckInactiveGameObjects();
                            break;
                        }
                    }
                }
            }
        }
        if (CompareTag("pencil"))
        {
            pencil.transform.position = worldPosition + pencilState;
            mycollider.transform.position = worldPosition;
        }
    }

    void CheckInactiveGameObjects()
    {

        if (!attachedobject[0].activeSelf && !attachedobject[1].activeSelf && !attachedobject[2].activeSelf && !attachedobject[3].activeSelf)
        {
            DelayUpdate();
            Debug.Log("completed");
        }
        else
        {
            Debug.Log("not complete");
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

        Fill.fillAmount = Mathf.Clamp01(Fill.fillAmount + 0.1428571428571429f);
    }

    public void TitleUpdateScene()
    {
        this.gameObject.SetActive(false);
        Test_scenes[test_counter].SetActive(true);
    }

    public void DelayUpdate()
    {
        Invoke("UpdateScene", 1f);
    }

    public void UpdateScene()
    {
        foreach (Button button in button)
        {
            button.interactable = false;
        }

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

        PlayerPrefs.SetInt("Test Score", Test_Score);
        PlayerPrefs.SetInt("Current Test", Level);
        Debug.Log("Score: " + Test_Score);
    }

    public void NextAnimal(GameObject gameObject)
    {
        if (gameObject.name == "Pre-Test 8" || gameObject.name == "Post-Test 8")
        {
            test_counter++;
            Test_scenes[test_counter].SetActive(true);
        }

        else
        {
            Test_scenes[test_counter].SetActive(false);
            test_counter++;
            Test_scenes[test_counter].SetActive(true);
        }
    }

    public void correct_Button_But_No_Increment()
    {
        Test_Score++;
    }

    public void Add_Point()
    {
        Test_Score++;
        Debug.Log("Score: " + Test_Score);
        DelayUpdate();
    }

    public void CountObjects(Button objectClicked)
    {
        object_counter++;
        objectClicked.interactable = false;

        if (objectClicked.name == "damo")
        {
            attachedobject[0].SetActive(true);
        }

        else if (objectClicked.name == "seeds")
        {
            attachedobject[4].SetActive(true);
        }

        else if (objectClicked.name == "bulate")
        {
            attachedobject[2].SetActive(true);
        }

        else if (objectClicked.name == "trunk")
        {
            Test_Score++;
            attachedobject[3].SetActive(true);
        }

        else if (objectClicked.name == "roots")
        {
            Test_Score++;
            attachedobject[1].SetActive(true);
        }

        else if (objectClicked.name == "sanga")
        {
            Test_Score++;
            attachedobject[5].SetActive(true);
        }

        if (object_counter == 3)
        {
            DelayUpdate();
        }
    }

    public void DisableScene(GameObject disableScene)
    {
        disableScene.SetActive(false);
    }

    public void EnableButton()
    {
        foreach (Button button in button)
        {
            button.interactable = true;
        }
    }
}
