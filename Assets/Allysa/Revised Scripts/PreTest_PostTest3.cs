using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

//using static System.Net.Mime.MediaTypeNames;
//using UnityEngine.Networking;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEngine.Playables;
using System;
using System.Reflection;


public class PreTest_PostTest3 : MonoBehaviour
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
    private Vector3 pencilRaise = new Vector3(125, 140, 0);
    private Vector3 pencilWrite = new Vector3(105, 120, 0);


    private Vector3 initialPosition = new Vector3(0, 0, 0);
    private Vector3 gameObjectState;
    public List<Button> button;
    //public Button next;

    private HashSet<string> tracedPoints = new HashSet<string>();
    private int score = 0;
    public int totalTracingPoints = 0;

    //theme3?
    public ScrollRect[] scrollRects;
    public Dictionary<int, string> visibleImageNames = new Dictionary<int, string>();
    public List<Image> imageList;
    private int Counter = 1;
    public List<GameObject> gameObjects;
    [SerializeField] private List<PlayableDirector> playableDirector;
    public List<Collider2D> colliderAreas = new List<Collider2D>();




    private float rotationSpeed = 300f;
    private bool isFlipping = false;
    public List<GameObject> front_card;
    private static int flipCount = 0;
    private GameObject target_object;
    private static int total_faceCard = 0;

    private static List<(Button, GameObject)> flippedCards = new List<(Button, GameObject)>();

    private AudioSource audioSource;
    private static int objectCounter = 0;
    //private static bool bgMusicPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        test_audiomanager = FindObjectOfType<Audio_Manager>();

        if (gameObject.name == "Pre-Test_SceneManager" || gameObject.name == "Post-Test_SceneManager")
        {
            test_counter = 0;
            Test_Score = 0;
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


        //theme3
        for (int i = 0; i < scrollRects.Length; i++)
        {
            if (scrollRects[i] != null)
            {
                int index = i;
                scrollRects[i].onValueChanged.AddListener((position) => OnScrollChanged(index, position));
                visibleImageNames[0] = "hospital";
                visibleImageNames[1] = "police station";
                visibleImageNames[2] = "fire station";
            }
        }
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

            else if (CompareTag("Draggable"))
            {
                gameObjectState = initialPosition;
                gameObjects[0].transform.position = worldPosition + gameObjectState;
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

            else if (CompareTag("Draggable"))
            {
                gameObjectState = initialPosition;

                Collider2D[] Colliders = Physics2D.OverlapPointAll(gameObjects[1].transform.position);

                foreach (Collider2D area in Colliders)
                {
                    if (area.CompareTag("Draggable") && area.name == "plant1 collider")
                    {

                        playableDirector[1].time = 15.65f;
                            playableDirector[1].Play();
                    }

                    else if (area.CompareTag("Draggable") && area.name == "plant2 collider")
                    {
                        Test_Score++;
                        playableDirector[1].time = 5.4f;
                            playableDirector[1].Play();
                    }

                }
            }
        }

        if (CompareTag("pencil"))
        {
            pencil.transform.position = worldPosition + pencilState;
            mycollider.transform.position = worldPosition;
        }

        //------

        bool matched = false;

        if (flipCount == 2)
        {
            foreach (var card in flippedCards)
            {
                bool card1Exist = flippedCards.Any(card => card.Item1.name == "card1");
                bool card2Exist = flippedCards.Any(card => card.Item1.name == "card2");
                bool card3Exist = flippedCards.Any(card => card.Item1.name == "card3");
                bool card4Exist = flippedCards.Any(card => card.Item1.name == "card4");
                bool card5Exist = flippedCards.Any(card => card.Item1.name == "card5");
                bool card6Exist = flippedCards.Any(card => card.Item1.name == "card6");

                if (card1Exist && card5Exist ||
                    card2Exist && card3Exist ||
                    card4Exist && card6Exist)
                {
                    matched = true;
                    flipCount = 0;
                    flippedCards.Clear();
                    total_faceCard++;
                    Test_Score++;
                    Debug.Log("matched!");
                    FaceCard_Checker();
                    Debug.Log(Test_Score);
                    break;

                }
            }

            if (!matched)
            {
                total_faceCard++;
                FaceCard_Checker();
                flipCount = 0;
                flippedCards.Clear();
            }
        }

        if (flipCount == 0)
        {
            isFlipping = false;
        }

        int index = 0;

        if (gameObject.name == "Pre-Test_SceneManager" || gameObject.name == "Post-Test_SceneManager")
        {

            PlayableDirector PanelplayableDirector;

            if (PlayerPrefs.HasKey("CurrentPanel"))
            {
                index = PlayerPrefs.GetInt("CurrentPanel");
                Debug.Log("Panel" + PlayerPrefs.GetInt("CurrentPanel"));
                PanelplayableDirector = Test_timelines[index].GetComponent<PlayableDirector>();
            }
            else
            {
                PanelplayableDirector = Title_timeline.GetComponent<PlayableDirector>();
            }

            if (PlayerPrefs.GetString("Paused") == "True")
            {
                if (Test_scenes[index].name != "Pre-Test 1" && Test_scenes[index].name != "Post-Test 1")
                {
                    PanelplayableDirector.Pause();
                    if (Test_scenes[index].name == "Pre-Test 2" || Test_scenes[index].name == "Post-Test 2")
                    {
                        Tracing_objects[0].SetActive(false);
                    }
                    else if (Test_scenes[index].name == "Pre-Test 4" || Test_scenes[index].name == "Post-Test 4")
                    {
                        Tracing_objects[1].SetActive(false);
                    }
                    else if (Test_scenes[index].name == "Pre-Test 8" || Test_scenes[index].name == "Post-Test 8")
                    {
                        Tracing_objects[2].SetActive(false);
                    }
                    else if (Test_scenes[index].name == "Pre-Test 3.5" || Test_scenes[index].name == "Post-Test 3.5")
                    {
                        Tracing_objects[3].SetActive(false);
                        Tracing_objects[4].SetActive(false);
                    }
                }
            }
            else
            {
                //PanelplayableDirector.Resume();
                if (Test_scenes[index].name != "Pre-Test 1" && Test_scenes[index].name != "Post-Test 1")
                {
                    PanelplayableDirector.Resume();
                    if (Test_scenes[index].name == "Pre-Test 2" || Test_scenes[index].name == "Post-Test 2")
                    {
                        Tracing_objects[0].SetActive(true);
                    }
                    else if (Test_scenes[index].name == "Pre-Test 4" || Test_scenes[index].name == "Post-Test 4")
                    {
                        Tracing_objects[1].SetActive(true);
                    }
                    else if (Test_scenes[index].name == "Pre-Test 8" || Test_scenes[index].name == "Post-Test 8")
                    {
                        Tracing_objects[2].SetActive(true);
                    }
                    else if (Test_scenes[index].name == "Pre-Test 3.5" || Test_scenes[index].name == "Post-Test 3.5")
                    {
                        Tracing_objects[3].SetActive(true);
                        Tracing_objects[4].SetActive(true);
                    }
                }
            }
        }
    }
    

    private void FaceCard_Checker()
    {
        if (total_faceCard == 3)
        {
            DelayUpdate();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tracing Point") && !tracedPoints.Contains(other.gameObject.name))
        {
            if (scene.name == "Pre-Test 3.5" || scene.name == "Post-Test 3.5")
            {
                score++;
                Debug.Log("points: " + score);

                if (other.gameObject.name == "dot1 (31)" && score >= 32)
                {
                    Test_Score++;
                    DelayUpdate();
                }

                else if (other.gameObject.name == "dot1 (45)" && score >= 30)
                {
                    DelayUpdate();
                }
            }

            else
            {
                tracedPoints.Add(other.gameObject.name);
                score++;
                Debug.Log("points: " + score);
            }
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
        Test_Score ++;
        DelayUpdate();
    }




    ////
    ///

    void OnScrollChanged(int index, Vector2 position)
    {
        UnityEngine.UI.Image visibleImage = GetVisibleImage(scrollRects[index]);

        if (visibleImage != null)
        {
            if (visibleImageNames.ContainsKey(index))
            {
                visibleImageNames[index] = visibleImage.gameObject.name;
            }
            else
            {
                visibleImageNames.Add(index, visibleImage.gameObject.name);
            }
        }
    }

    private UnityEngine.UI.Image GetVisibleImage(ScrollRect scrollRect)
    {
        RectTransform scrollRectTransform = scrollRect.viewport ?? scrollRect.GetComponent<RectTransform>();
        RectTransform content = scrollRect.content;

        foreach (RectTransform child in content)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(scrollRectTransform, child.position, null))
            {
                return child.GetComponent<UnityEngine.UI.Image>();
            }
        }
        return null;
    }
    
    public void OnSubmit()
    {
        if (Counter == 1)
        {
            if (visibleImageNames.ContainsKey(0) && visibleImageNames[0] == "hospital" &&
                       visibleImageNames.ContainsKey(1) && visibleImageNames[1] == "hospital" &&
                       visibleImageNames.ContainsKey(2) && visibleImageNames[2] == "hospital")
            {
                Counter++;
                Debug.Log("counter: " + Counter);
                Test_Score++;
                playableDirector[0].Play();
                button[0].gameObject.SetActive(false);
                Debug.Log("score: " + Test_Score);
            }
            else
            {
                Counter++;
                playableDirector[0].Play();
                button[0].gameObject.SetActive(false);
                Debug.Log("score: " + Test_Score);
            }
        }

        else if (Counter == 2)
        {
            if (visibleImageNames.ContainsKey(0) && visibleImageNames[0] == "fire station" &&
                       visibleImageNames.ContainsKey(1) && visibleImageNames[1] == "fire station" &&
                       visibleImageNames.ContainsKey(2) && visibleImageNames[2] == "fire station")
            {
                Counter++;
                Debug.Log("counter: " + Counter);
                Test_Score++;
                playableDirector[0].Play();
                button[0].gameObject.SetActive(false);
                Debug.Log("score: " + Test_Score);
            }
            else
            {
                Counter++;
                playableDirector[0].Play();
                button[0].gameObject.SetActive(false);
                Debug.Log("score: " + Test_Score);
            }
            Debug.Log(Counter);
        }

        else if (Counter == 3)
        {
            if (visibleImageNames.ContainsKey(0) && visibleImageNames[0] == "police station" &&
                       visibleImageNames.ContainsKey(1) && visibleImageNames[1] == "police station" &&
                       visibleImageNames.ContainsKey(2) && visibleImageNames[2] == "police station")
            {
                Add_Point();
            }
            else
            {
                DelayUpdate();
            }
        }
    }

    public void PauseTimeline(int index)
    {
        playableDirector[index].Pause();
        Debug.Log(playableDirector[index].name);

        if (playableDirector[index].name == "Pre-test 3.1" || playableDirector[index].name == "Post-test 3.1")
        {
            if (Counter == 1)
            {
                gameObjects[3].SetActive(false);
                button[0].gameObject.SetActive(true);
            }

            else if (Counter == 2)
            {
                gameObjects[1].SetActive(false);
                button[0].gameObject.SetActive(true);
            }

            else 
            {
                gameObjects[2].SetActive(false);
                button[0].gameObject.SetActive(true);
            }
        }
    }

    public void StartFlip(Button flipbutton)
    {
        if (isFlipping) return;

        if (flipbutton.name == "card1")
        {
            target_object = front_card[0];
        }
        else if (flipbutton.name == "card2")
        {
            target_object = front_card[1];
        }
        else if (flipbutton.name == "card3")
        {
            target_object = front_card[2];
        }
        else if (flipbutton.name == "card4")
        {
            target_object = front_card[3];
        }
        else if (flipbutton.name == "card5")
        {
            target_object = front_card[4];
        }
        else
        {
            target_object = front_card[5];
        }

        StartCoroutine(FlipCard(flipbutton, target_object));
        flippedCards.Add((flipbutton, target_object));
    }

    private IEnumerator FlipCard(Button cardbutton, GameObject picture)
    {
        if (isFlipping) yield break;

        isFlipping = true;
        float rotationAmount = 0f;
        Quaternion startRotation1 = cardbutton.transform.rotation;
        Quaternion endRotation1 = startRotation1 * Quaternion.Euler(0, 180, 0);

        Quaternion startRotation2 = picture.transform.rotation;
        Quaternion endRotation2 = startRotation2 * Quaternion.Euler(0, 180, 0);

        while (rotationAmount < 1f)
        {
            rotationAmount += Time.deltaTime * rotationSpeed / 180f;
            transform.rotation = Quaternion.Slerp(startRotation1, endRotation1, rotationAmount);
            transform.rotation = Quaternion.Slerp(startRotation2, endRotation2, rotationAmount);

            if (rotationAmount >= 0.5f)
            {
                cardbutton.gameObject.SetActive(false);
                picture.SetActive(true);
                flipCount++;
            }
            yield return null;
        }

        cardbutton.transform.rotation = endRotation1;
        picture.transform.rotation = endRotation2;
        isFlipping = false;
    }



    public void AfterTimeline()
    {
        if (this.gameObject.name == "Pre-Test 3.6" || this.gameObject.name == "Post-Test 3.6")
        {
            for (int j = 0; j < button.Count; j++)
            {
                button[j].interactable = true;
            }


            for (int i = 0; i < front_card.Count; i++)
            {
                front_card[i].SetActive(false);

                front_card[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                Image image = front_card[i].GetComponent<Image>();

                if (image != null)
                {
                    image.color = new Color32(255, 255, 255, 255);
                }

                AudioSource audioSource1 = front_card[i].GetComponent<AudioSource>();
                if (audioSource1 != null)
                {
                    audioSource1.enabled = true;
                }
            }
        }
    }



    public void CorrectObject()
    {
        if (this.gameObject.name == "Teddy Bear")
        {
            gameObjects[5].SetActive(true);
        }

        else if (this.gameObject.name == "treasure chest")
        {
            gameObjects[6].SetActive(true);
        }

        else if (this.gameObject.name == "Clock")
        {
            gameObjects[8].SetActive(true);
        }

        else if (this.gameObject.name == "atis")
        {
            gameObjects[10].SetActive(true);
        }

        else if (this.gameObject.name == "crayon")
        {
            gameObjects[11].SetActive(true);
        }

        else if (this.gameObject.name == "cone")
        {
            gameObjects[9].SetActive(true);
        }

        else if (this.gameObject.name == "pencil")
        {
            gameObjects[4].SetActive(true);
        }

        else if (this.gameObject.name == "paper")
        {
            gameObjects[7].SetActive(true);
        }

        Test_Score++;
        objectCounter++;
        ObjectCount();
        Debug.Log(objectCounter);
    }

    public void WrongObject()
    {
        if (this.gameObject.name == "Teddy Bear")
        {
            gameObjects[5].SetActive(true);
        }

        else if (this.gameObject.name == "treasure chest")
        {
            gameObjects[6].SetActive(true);
        }

        else if (this.gameObject.name == "Clock")
        {
            gameObjects[8].SetActive(true);
        }

        else if (this.gameObject.name == "atis")
        {
            gameObjects[10].SetActive(true);
        }

        else if (this.gameObject.name == "crayon")
        {
            gameObjects[11].SetActive(true);
        }

        else if (this.gameObject.name == "cone")
        {
            gameObjects[9].SetActive(true);
        }

        else if (this.gameObject.name == "pencil")
        {
            gameObjects[4].SetActive(true);
        }

        else if (this.gameObject.name == "paper")
        {
            gameObjects[7].SetActive(true);
        }

        objectCounter++;
        ObjectCount();
        Debug.Log(objectCounter);
    }

    private void ObjectCount()
    {
        if (objectCounter == 3)
        {
            DelayUpdate();
        }
    }

    public void EnableButton()
    {
        foreach (Button button in button)
        {
            button.interactable = true;
        }
    }
 
    public void Stop_timeline()
    {
        playableDirector[1].Stop();
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
