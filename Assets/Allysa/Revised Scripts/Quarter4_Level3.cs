using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Quarter4_Level3 : MonoBehaviour
{
    private static int Scene_counter = 0;
    public List<GameObject> scenes;
    public GameObject Nxtbutton;

    private Vector3 pencilRaise = new Vector3(105, 120, 0);
    private Vector3 pencilWrite = new Vector3(85, 100, 0);
    private Vector3 pencilState;
    private Vector3 draggableObjectState;
    private Vector3 offset;
    private Vector3 initialPosition = new Vector3(0, 0, 0);

    public List<GameObject> gameobjects;
    public List<Collider2D> ListofColliders;
    private Transform draggable;
    private static int wrongAnswer = 0;
    
    public Image Fill;

    private HashSet<string> tracedPoints = new HashSet<string>();
    private int score = 0;
    public int totalTracingPoints = 0;

    private Audio_Manager audioManager_theme4;

    private static bool increment = false;
    private static bool increment_wrongAnswer = false;
    private float incrementAmount;
    public TextMeshProUGUI text;

    public GameObject gameMenu;
    public GameObject[] tracingObjects;
    //private static bool bgMusicPlayed = false;



    void Start()
    {
        audioManager_theme4 = FindObjectOfType<Audio_Manager>();

        //if (!bgMusicPlayed)
        //{
        //    if (audioManager_theme4 != null)
        //    {
        //        audioManager_theme4.scene_bgmusic(0.084f);
        //        bgMusicPlayed = true;
        //    }
        //}
    }

    public void DelayUpdate()
    {
        Invoke("UpdateScene", 1f);
    }

    public void UpdateScene()
    {
        scenes[Scene_counter].SetActive(false);
        Scene_counter++;
        scenes[Scene_counter].SetActive(true);

        if (Scene_counter == 3 || Scene_counter == 6)
        {
            Nxtbutton.SetActive(false);
        }

        if (Scene_counter == 13)
        {
            Nxtbutton.SetActive(false);
            Scene_counter++;
            scenes[Scene_counter].SetActive(true);
            //audioManager_theme4.assessment_bgmusic(0.5f);
        }

        if (Scene_counter == 16)
        {
            Scene_counter++;
            scenes[Scene_counter].SetActive(true);
        }

        if (Scene_counter == 21)
        {
            //audioManager_theme4.Stop_backgroundMusic2();
            scenes[13].SetActive(false);
            scenes[16].SetActive(false);
            Show_Stars();
        }
    }

    void Update()
    {
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
                gameobjects[0].GetComponent<CircleCollider2D>().enabled = true;
                pencilState = pencilWrite;

                GameObject pencilMask = Instantiate(gameobjects[1], worldPosition, Quaternion.identity);
                pencilMask.transform.SetParent(gameobjects[2].transform);
            }

            if (CompareTag("Draggable"))
            {
                if (draggable != null)
                {
                    draggable.position = worldPosition + offset;
                }
            }
        }

        else
        {
            if (CompareTag("pencil"))
            {
                gameobjects[0].GetComponent<CircleCollider2D>().enabled = false;
                pencilState = pencilRaise;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (CompareTag("pencil"))
            {
                gameobjects[0].GetComponent<CircleCollider2D>().enabled = false;
                pencilState = pencilRaise;
            }

            if (CompareTag("Draggable"))
            {
                if (draggable != null)
                {
                    draggable.position = worldPosition + draggableObjectState;

                    Collider2D[] Colliders = Physics2D.OverlapPointAll(draggable.transform.position);

                    foreach (Collider2D area in Colliders)
                    {
                        if (ListofColliders.Contains(area))
                        {
                            if (area.name == "setting 1" && draggable.name == "baka" || area.name == "Bb container" && draggable.name == "baboy")
                            {
                                audioManager_theme4.Correct();
                                draggable.gameObject.SetActive(false);
                                gameobjects[0].SetActive(true);
                                fillCorrect();
                                CheckInactiveGameObjects();
                                Debug.Log("CORRECT!");
                                break;
                            }

                            else if (area.name == "setting 2" && draggable.name == "baboy" || area.name == "Bb container" && draggable.name == "baka")
                            {
                                audioManager_theme4.Correct();
                                draggable.gameObject.SetActive(false);
                                gameobjects[1].SetActive(true);
                                fillCorrect();
                                CheckInactiveGameObjects();
                                Debug.Log("CORRECT!");
                                break;
                            }

                            else if (area.name == "setting 3" && draggable.name == "palaka" || area.name == "Pp container " && draggable.name == "palaka")
                            {
                                audioManager_theme4.Correct();
                                draggable.gameObject.SetActive(false);
                                gameobjects[2].SetActive(true);
                                fillCorrect();
                                CheckInactiveGameObjects();
                                Debug.Log("CORRECT!");
                                break;
                            }

                            else if (area.name == "setting 3" && draggable.name == "teri" || area.name == "Pp container " && draggable.name == "teri")
                            {
                                audioManager_theme4.Correct();
                                draggable.gameObject.SetActive(false);
                                gameobjects[3].SetActive(true);
                                fillCorrect();
                                CheckInactiveGameObjects();
                                Debug.Log("CORRECT!");
                                break;
                            }


                            else
                            {
                                if (!increment_wrongAnswer)
                                {
                                    wrongAnswer++;
                                    increment_wrongAnswer = true;
                                }

                                audioManager_theme4.Wrong();

                                if (draggable.name == "palaka")
                                {
                                    draggable.transform.position = new Vector3(695, 128, 90);
                                }

                                else if (draggable.name == "baboy")
                                {
                                    draggable.transform.position = new Vector3(831, 171, 90);
                                }

                                else if (draggable.name == "rabo")
                                {
                                    draggable.transform.position = new Vector3(1000, 183, 90);
                                }

                                else
                                {
                                    draggable.transform.position = new Vector3(1168, 169, 90);
                                }

                                Debug.Log("wrong!");
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (CompareTag("pencil"))
        {
            gameobjects[3].transform.position = worldPosition + pencilState;
            gameobjects[0].transform.position = worldPosition;
        }

        PlayableDirector PanelPlayableDirector;
        //int index = PlayerPrefs.GetInt("CurrentPanel");
        //Debug.Log(index);
        //if (!scenes[index].activeSelf) {
        //}
        int index = 0;
        string objectName = gameObject.name;
        string tlName = null;
        if (objectName == "Scene Manager" && scenes.Count > 0)
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                if (scenes[i].activeSelf)
                {
                    index = i + 1;
                    if (index != 14 && index != 17)
                    {
                        Debug.Log(index);
                        break;
                    }
                }
            }

            tlName = "scene 4." + (index);
            Debug.Log(tlName);
            if (index < 22)
            {
                if (scenes[index - 1].name == "Scene 5")
                {
                    if (PlayerPrefs.GetString("Paused") == "True")
                    {
                        tracingObjects[0].SetActive(false);
                    }
                    else
                    {
                        tracingObjects[0].SetActive(true);
                    }
                }
                else if (scenes[index - 1].name == "Scene 8")
                {
                    if (PlayerPrefs.GetString("Paused") == "True")
                    {
                        tracingObjects[1].SetActive(false);
                    }
                    else
                    {
                        tracingObjects[1].SetActive(true);
                    }
                }
                else
                {
                    if (index-1 >= 14)
                    {
                        tlName = "assessment" + (index-14);
                        if (index-1 == 17)
                        {
                            tlName = "assessment3_baboy";
                        }
                        else if (index-1 == 18)
                        {
                            tlName = "assessment3_baka";
                        }
                        else if (index - 1 == 19)
                        {
                            tlName = "assessment3_teri";
                        }
                        else if (index - 1 == 20)
                        {
                            tlName = "assessment3_palaka";
                        }
                        Debug.Log(tlName);
                        Debug.Log(scenes[index-1].name);
                    }   
                    GameObject timeline = scenes[index-1].transform.Find(tlName).gameObject;
                    PanelPlayableDirector = timeline.GetComponent<PlayableDirector>();
                    if (PlayerPrefs.GetString("Paused") == "True")
                    {
                        PanelPlayableDirector.Pause();
                    }
                    else if (PlayerPrefs.GetString("Paused") == "False")
                    {
                        PanelPlayableDirector.Resume();
                        PlayerPrefs.DeleteKey("Paused");
                    }
                }
            }
            else
            {
                gameMenu.SetActive(false);
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
            CheckCompletion();
        }
    }

    void CheckCompletion()
    {
        //    if (gameobjects[2].name == "Scene 5" || gameobjects[2].name == "Scene 8")
        //    {
        if (tracedPoints.Count >= totalTracingPoints)
            {
                audioManager_theme4.Correct();
                Nxtbutton.SetActive(true);
            }
        //}
    }

    void CheckInactiveGameObjects()
    {

        if (gameobjects[0].activeSelf && gameobjects[1].activeSelf && gameobjects[2].activeSelf && gameobjects[3].activeSelf)
        {
            IncrementFillAmount(0.000001f);
            DelayUpdate();
        }

        else
        {
            Debug.Log("not yet complete");
        }
    }

    public void Wrong_click()
    {
        audioManager_theme4.Wrong();
        wrongAnswer ++;
        Debug.Log("wrong: " +  wrongAnswer);
    }

    public void Correct_click()
    {
        increment = false;
        audioManager_theme4.Correct();
        fillCorrect();
        DelayUpdate();
    }

    public void IncrementFillAmount(float amount)
    {
        Fill.fillAmount = Mathf.Clamp01(Fill.fillAmount + amount);
    }

    private void progressFillChecker()
    {
        if (Fill.fillAmount >= 0.3333333333333333f && Fill.fillAmount < 0.6666666666666667f)
        {
            if (gameobjects.Count == 7)
            {
                gameobjects[4].SetActive(false);
            }

            else if (gameobjects.Count == 11)
            {
                gameobjects[0].SetActive(false);
            }
        }

        else if (Fill.fillAmount >= 0.6666666666666667f && Fill.fillAmount < 1f)
        {
            if (gameobjects.Count == 7)
            {
                gameobjects[5].SetActive(false);
            }

            else if (gameobjects.Count == 11)
            {
                gameobjects[1].SetActive(false);
            }
        }

        else if (Mathf.Approximately(Fill.fillAmount, 1f))
        {
            if (gameobjects.Count == 7)
            {
                gameobjects[6].SetActive(false);
            }

            else if (gameobjects.Count == 11)
            {
                gameobjects[2].SetActive(false);
            }
        }
    }

    void fillCorrect()
    {
        if (!increment)
        {
            if (wrongAnswer > 0)
            {
                if (ListofColliders.Count == 3)
                {
                    incrementAmount = 0.0277777777777778f;
                }

                else if (ListofColliders.Count == 2)
                {
                    incrementAmount = 0.0416666666666667f;
                }

                Debug.Log("wrong: " + wrongAnswer);
                IncrementFillAmount((0.0833333333333333f - (incrementAmount * wrongAnswer)));
                progressFillChecker();
                increment = true;
                increment_wrongAnswer = false;
                wrongAnswer = 0;
            }

            else
            {
                IncrementFillAmount(0.0833333333333333f);
                progressFillChecker();
                increment = true;
            }
        }
    }

    void Show_Stars()
    {
        Debug.Log(this.gameObject);

        if (Fill.fillAmount < 0.3333333333333333f)
        {
            gameobjects[3].SetActive(true);
            gameobjects[7].SetActive(false);
            gameobjects[8].SetActive(false);
            gameobjects[9].SetActive(false);
            gameobjects[25].SetActive(true);
            //gameobjects[26].SetActive(false);
            PlayerPrefs.SetInt("Delay Time", 4);
        }

        else if (Fill.fillAmount >= 0.3333333333333333f && Fill.fillAmount < 0.6666666666666667f)
        {
            gameobjects[4].SetActive(true);
            gameobjects[9].SetActive(false);
            PlayerPrefs.SetInt("Delay Time", 4);
        }

        else if (Fill.fillAmount >= 0.6666666666666667f && Fill.fillAmount < 1f)
        {
            gameobjects[5].SetActive(true);
            PlayerPrefs.SetInt("Delay Time", 4);
        }

        else if (Mathf.Approximately(Fill.fillAmount, 1f))
        {
            gameobjects[6].SetActive(true);
            PlayerPrefs.SetInt("Delay Time", 8);
        }

        PlayerPrefs.SetFloat("Theme4 Score", Fill.fillAmount);
    }
}

