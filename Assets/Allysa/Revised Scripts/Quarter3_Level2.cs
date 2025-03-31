using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
public class Quarter3_Level2 : MonoBehaviour
{
    private static int Scene_counter = 0;
    public List<GameObject> scenes;
    private float rotationSpeed = 300f;
    private bool isFlipping = false;
    public List<GameObject> front_card;
    private static int flipCount = 0;
    private GameObject target_object;
    private static int total_matched = 0;

    private static List<(Button, GameObject)> flippedCards = new List<(Button, GameObject)>();
    private Vector3 initialPosition = new Vector3(0, 0, 0);
    private Vector3 hammerState;
    private Vector3 treatmentItemsState;
    public List<Collider2D> colliderAreas = new List<Collider2D>();
    public List<Animator> objectAnimation;
    public List<Button> button;
    private static int disinpect_step = 0;


    private Vector3 pencilWrite = new Vector3(105, 120, 0);
    private Vector3 pencilState;
    private Vector3 pencilRaise = new Vector3(125, 140, 0);
    private HashSet<string> tracedPoints = new HashSet<string>();
    private int score = 0;
    private int totalTracingPoints = 29;

    public ScrollRect[] scrollRects;
    public Dictionary<int, string> visibleImageNames = new Dictionary<int, string>();
    public List<Image> imageList;
    private int Counter = 1;
    private bool isPaused = false;

    private static int wrong_Click = 1;
    private static int object_count = 0;

    private Audio_Manager audioManager4;
    private AudioSource audioSource;
    private static bool isPlayed = false;
    //[SerializeField] private List<AudioClip> audios;

    public List<TextMeshProUGUI> text;
    //private static bool bgMusicPlayed = false;
    private float fillamount;

    public GameObject tracingPath;
    public GameObject gameMenu;

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private THEME1_LEVEL1_REQUESTS requestsManager;



    void Start()
    {
        requestsManager = FindObjectOfType<THEME1_LEVEL1_REQUESTS>();

        audioManager4 = FindObjectOfType<Audio_Manager>();
        if (rotateButton != null && CorrectImage != null)
        {
            rotateButton.onClick.AddListener(Rotate);
        }

        objectTransform = GetComponent<Transform>();
        originalPosition = objectTransform.position;

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

        audioSource = GetComponent<AudioSource>();

        //if (!bgMusicPlayed)
        //{
        //    if (audioManager4 != null)
        //    {
        //        audioManager4.scene_bgmusic(1f);
        //        bgMusicPlayed = true;
        //    }
        //}
    }

    public void UpdateScene()
    {
        scenes[Scene_counter].SetActive(false);
        Scene_counter++;
        scenes[Scene_counter].SetActive(true);

        if (Scene_counter == 1)
        {
            button[1].gameObject.SetActive(true);
        }

        else if (Scene_counter == 2 || Scene_counter == 4 || Scene_counter == 6 || Scene_counter == 8 || Scene_counter == 10)
        {
            button[1].gameObject.SetActive(false);
        }

        //else if (Scene_counter == 11)
        //{
        //    audioManager4.assessment_bgmusic(0.5f);
        //}

        else if (Scene_counter == 12)
        {
            Scene_counter++;
            scenes[Scene_counter].SetActive(true);
        }

    }

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

    private Image GetVisibleImage(ScrollRect scrollRect)
    {
        RectTransform scrollRectTransform = scrollRect.viewport ?? scrollRect.GetComponent<RectTransform>();
        RectTransform content = scrollRect.content;

        foreach (RectTransform child in content)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(scrollRectTransform, child.position, null))
            {
                return child.GetComponent<Image>();
            }
        }
        return null;
    }

    public void OnSubmit()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!visibleImageNames.ContainsKey(i) || string.IsNullOrEmpty(visibleImageNames[i]))
            {
                UnityEngine.UI.Image visibleImage = GetVisibleImage(scrollRects[i]);
                if (visibleImage != null)
                {
                    visibleImageNames[i] = visibleImage.gameObject.name;
                }
            }
        }

        foreach (var kvp in visibleImageNames)
        {
            Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
        }

        if (Counter == 1)
        {
            if (visibleImageNames.ContainsKey(0) && visibleImageNames[0] == "hospital" &&
                       visibleImageNames.ContainsKey(1) && visibleImageNames[1] == "hospital" &&
                       visibleImageNames.ContainsKey(2) && visibleImageNames[2] == "hospital")
            {
                Counter++;
                Debug.Log("counter: " + Counter);
                audioManager4.Correct();
                Invoke("PlaySoundEffect", 0.2f);


                if (wrong_Click > 0)
                {
                    float fillamount = 0.1111111111111111f / wrong_Click;
                    IncrementFillAmount(fillamount);
                    wrong_Click = 0;
                }
                else
                {
                    IncrementFillAmount(0.1111111111111111f);
                }

                wrong_Click = 0;
                Invoke("ResumeTimeline", 2);
                gameobjects[12].SetActive(true);
            }
            else
            {
                wrong_Click++;
                Debug.Log("try again");
                audioManager4.Wrong();
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
                audioManager4.Correct();
                Invoke("PlaySoundEffect", 0.2f);
                if (wrong_Click > 0)
                {
                    float fillamount = 0.1111111111111111f / wrong_Click;
                    IncrementFillAmount(fillamount);
                    wrong_Click = 0;
                }
                else
                {
                    IncrementFillAmount(0.1111111111111111f);
                }
                wrong_Click = 0;
                Invoke("ResumeTimeline", 2);
                gameobjects[13].SetActive(true);
            }
            else
            {
                Debug.Log("try again");
                wrong_Click++;
                audioManager4.Wrong();
            }
        }
        else
        {
            if (visibleImageNames.ContainsKey(0) && visibleImageNames[0] == "police station" &&
                       visibleImageNames.ContainsKey(1) && visibleImageNames[1] == "police station" &&
                       visibleImageNames.ContainsKey(2) && visibleImageNames[2] == "police station")
            {
                audioManager4.Correct();
                Invoke("PlaySoundEffect", 0.2f);
                if (wrong_Click > 0)
                {
                    float fillamount = 0.1111111111111111f / wrong_Click;
                    IncrementFillAmount(fillamount);
                    wrong_Click = 0;
                }
                else
                {
                    IncrementFillAmount(0.1111111111111111f);
                }
                Invoke("UpdateScene", 1f);
                //audioManager4.Stop_backgroundMusic2();
                Invoke("Show_Stars", 1f);
            }
            else
            {
                Debug.Log("try again");
                wrong_Click++;
                audioManager4.Wrong();
            }
        }
    }

    void PlaySoundEffect()
    {
        if (Counter == 2)
        {
            audioManager4.SoundEffect(7);
        }
        else if (Counter == 3)
        {
            audioManager4.SoundEffect(8);
        }
        else 
        { 
            audioManager4.SoundEffect(9);
        }
    }

    static bool audioEnabled = false;

    private void Update()
    {
        if (CompareTag("attached audio source"))
        {
            if (this.gameObject.name == "Assessment 2")
            {
                if (playableDirector.time >= 5.2021)
                {
                    playableDirector.Pause();
                    audioSource = gameObject.GetComponent<AudioSource>();
                    audioSource.enabled = true;
                }
            }

            if (!audioSource.isPlaying && !audioSource.loop)
            {
                if (this.gameObject.name == "Scene4")
                {
                    button[0].interactable = true;
                }

                else if (this.gameObject.name == "Scene6")
                {
                    button[0].interactable = true;
                    button[1].interactable = true;
                    button[2].interactable = true;
                }

                else if (this.gameObject.name == "Scene8" && !isPlayed)
                {
                    hammer[1].gameObject.SetActive(true);
                    isPlayed = true;
                }

                else if (this.gameObject.name == "Scene10")
                {
                    gameobjects[0].SetActive(true);
                }

                else if (this.gameObject.name == "Assessment 2" && audioSource.enabled && !audioEnabled)
                {
                    AfterTimeline();
                    audioEnabled = true;
                }
            }
        }

        if (CompareTag("horizontal scroll"))
        {
            if (Counter == 1)
            {
                if (playableDirector != null && playableDirector.state == PlayState.Playing)
                {
                    if (playableDirector.time >= 9.461)
                    {
                        playableDirector.Pause();
                        gameobjects[11].SetActive(false);
                        button[0].gameObject.SetActive(true);
                        isPaused = true;
                    }
                }
            }
            else if (Counter == 2)
            {
                if (playableDirector != null && playableDirector.state == PlayState.Playing)
                {
                    if (playableDirector.time >= 11.7)
                    {
                        playableDirector.Pause();
                        gameobjects[12].SetActive(false);
                        button[0].gameObject.SetActive(true);
                        isPaused = true;
                    }
                }
            }
            else
            {
                if (playableDirector != null && playableDirector.state == PlayState.Playing)
                {
                    if (playableDirector.time >= 14.0324)
                    {
                        playableDirector.Pause();
                        gameobjects[13].SetActive(false);
                        button[0].gameObject.SetActive(true);
                        isPaused = true;
                    }
                }
            }
        }

        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane + 1;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (Input.GetMouseButton(0))
        {
            if (CompareTag("treatment"))
            {
                var animator = GetComponent<Animator>();

                if (disinpect_step == 0)
                {
                    treatmentItemsState = initialPosition;
                    button[0].transform.position = worldPosition + treatmentItemsState;
                }
                else if (disinpect_step == 1)
                {
                    treatmentItemsState = initialPosition;
                    button[1].transform.position = worldPosition + treatmentItemsState;
                }
                else
                {
                    treatmentItemsState = initialPosition;
                    button[2].transform.position = worldPosition + treatmentItemsState;
                }
            }

            else if (CompareTag("hammer"))
            {
                var animator = GetComponent<Animator>();
                animator.SetBool("martilyo", false);

                hammer[0].SetActive(true);
                hammer[1].SetActive(false);
                hammerState = initialPosition;
                hammer[0].transform.position = worldPosition + hammerState;
            }

            else if (CompareTag("pencil"))
            {
                gameobjects[10].GetComponent<CircleCollider2D>().enabled = true;
                pencilState = pencilWrite;

                GameObject pencilMask = Instantiate(gameobjects[9], worldPosition, Quaternion.identity);
                pencilMask.transform.SetParent(gameobjects[7].transform);
            }
        }

        else
        {
            if (CompareTag("pencil"))
            {
                gameobjects[10].GetComponent<CircleCollider2D>().enabled = false;
                pencilState = pencilRaise;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (CompareTag("treatment"))
            {
                treatmentItemsState = initialPosition;
                bool treatmentFound = false;

                if (disinpect_step == 0)
                {
                    button[0].transform.position = worldPosition + treatmentItemsState;
                    button[0].gameObject.SetActive(false);
                    Collider2D[] Colliders = Physics2D.OverlapPointAll(button[0].transform.position);

                    foreach (Collider2D area in Colliders)
                    {
                        if (area.CompareTag("treatment"))
                        {
                            //Debug.Log("button touched the collider on drop: " + area.name);
                            objectAnimation[0].enabled = true;
                            button[0].gameObject.SetActive(true);
                            playableDirector.stopped += Delay2seconds;
                            playableDirector.time = 0;
                            playableDirector.Play();
                            treatmentFound = true;
                            break;

                        }
                        
                    }

                    if (!treatmentFound)
                    {
                        button[0].gameObject.SetActive(true);
                        button[0].transform.position = new Vector3(-309, 170, 0);
                    }
                    
                }
                else if (disinpect_step == 1)
                {
                    button[1].transform.position = worldPosition + treatmentItemsState;
                    button[1].gameObject.SetActive(false);
                    Collider2D[] Colliders = Physics2D.OverlapPointAll(button[1].transform.position);

                    foreach (Collider2D area in Colliders)
                    {
                        if (area.CompareTag("treatment"))
                        {
                            objectAnimation[2].enabled = true;
                            button[1].gameObject.SetActive(true);
                            playableDirector.stopped += Delay2seconds;
                            playableDirector.time = 0;
                            playableDirector.Play();
                            treatmentFound = true;
                            break;
                        }
                    }

                    if (!treatmentFound)
                    {
                        button[1].gameObject.SetActive(true);
                        button[1].transform.position = new Vector3(-309, -19, 0);
                    }
                }
                else
                {
                    button[2].transform.position = worldPosition + treatmentItemsState;
                    //button[2].gameObject.SetActive(false);
                    Collider2D[] Colliders = Physics2D.OverlapPointAll(button[2].transform.position);

                    foreach (Collider2D area in Colliders)
                    {
                        if (area.CompareTag("treatment"))
                        {
                            audioManager4.SoundEffect(6);
                            StartCoroutine(Show_confetti());
                            treatmentFound = true;
                            break;
                        }
                    }

                    if (!treatmentFound)
                    {
                        button[2].gameObject.SetActive(true);
                        button[2].transform.position = new Vector3(-367, -201, 0);
                    }
                }
            }


            else if (CompareTag("hammer"))
            {
                hammerState = initialPosition;
                hammer[0].transform.position = worldPosition + hammerState;

                Collider2D[] colliders = Physics2D.OverlapPointAll(hammerTip.transform.position);

                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("nail"))
                    {
                        //Debug.Log("Hammer tip touched the nail on drop: " + collider.name);

                        if (collider.name == "nail4")
                        {
                            objectAnimation[1].SetBool("martilyo", true);
                            StartCoroutine(HandleNailHit(0));
                            colliderAreas[0].gameObject.SetActive(false);
                        }
                        else if (collider.name == "nail3")
                        {
                            objectAnimation[1].SetBool("martilyo", true);
                            StartCoroutine(HandleNailHit(1));
                            colliderAreas[1].gameObject.SetActive(false);
                        }
                        else if (collider.name == "nail2")
                        {
                            objectAnimation[1].SetBool("martilyo", true);
                            StartCoroutine(HandleNailHit(3));
                            colliderAreas[2].gameObject.SetActive(false);
                        }
                        else
                        {
                            objectAnimation[1].SetBool("martilyo", true);
                            StartCoroutine(HandleNailHit(2));
                            colliderAreas[3].gameObject.SetActive(false);
                        }
                        break;
                    }
                }
            }

            else if (CompareTag("pencil"))
            {
                gameobjects[10].GetComponent<CircleCollider2D>().enabled = false;
                pencilState = pencilRaise;
            }

        }

        if (CompareTag("pencil"))
        {
            gameobjects[8].transform.position = worldPosition + pencilState;
            gameobjects[10].transform.position = worldPosition;
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
                    total_matched++; 
                    Debug.Log("matched!");
                    Debug.Log(total_matched);
                    audioManager4.Invoke("Correct", 1);
                    StartCoroutine(DelayedIncrementFillAmount());
                    Invoke("matchedChecker", 2f);
                    break;

                }
            }

            if (!matched)
            {
                wrong_Click++;
                Invoke("check_cards", 0.5f);
                flipCount++;
                audioManager4.Invoke("Wrong", 1);
            }
        }

        if (flipCount == 0)
        {
            isFlipping = false;
        }

        //if (imageList.Count == 3)
        // {
        //     if (imageList[3].fillAmount >= 0.3333333333333333f && imageList[3].fillAmount < 0.6666666666666667f)
        //     {
        //         gameobjects[15].SetActive(false);
        //     }

        //     else if (imageList[3].fillAmount >= 0.6666666666666667f && imageList[3].fillAmount < 1f)
        //     {
        //         gameobjects[16].SetActive(false);
        //     }

        //     else if (Mathf.Approximately(imageList[3].fillAmount, 1f))
        //     {
        //         gameobjects[17].SetActive(false);
        //     }
        //}

        if (imageList.Count > 3)
        {
            if (imageList[3].fillAmount >= 0.3333333333333333f && imageList[3].fillAmount < 0.6666666666666667f)
            {
                if (gameobjects.Count == 3)
                { 
                    gameobjects[0].SetActive(false);
                }
                else if (gameobjects.Count == 27)
                {
                    gameobjects[15].SetActive(false);
                }
            }

            else if (imageList[3].fillAmount >= 0.6666666666666667f && imageList[3].fillAmount < 1f)
            {
                if (gameobjects.Count == 3)
                {
                    gameobjects[1].SetActive(false);
                }
                else if (gameobjects.Count == 27)
                {
                    gameobjects[16].SetActive(false);
                }
            }

            else if (Mathf.Approximately(imageList[3].fillAmount, 1f))
            {
                if (gameobjects.Count == 3)
                {
                    gameobjects[2].SetActive(false);
                }
                else if (gameobjects.Count == 27)
                {
                    gameobjects[17].SetActive(false);
                }
            }
        }

        PlayableDirector PanelPlayableDirector;
        //int index = PlayerPrefs.GetInt("CurrentPanel");
        //Debug.Log(index);
        //if (!scenes[index].activeSelf) {
        //}
        int index = 0;
        string objectName = gameObject.name;
        string tlName = null;
        if (objectName[..5] == "Scene" && scenes.Count > 0)
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                if (scenes[i].activeSelf)
                {
                    index = i+1;
                    if (index != 13)
                    {
                        break;
                    }
                }
            }
            
            tlName = "scene" + index + "_timeline";
            Debug.Log(tlName);
            if (index < 17)
            {
                GameObject timeline = scenes[index-1].transform.Find(tlName).gameObject;
                PanelPlayableDirector = timeline.GetComponent<PlayableDirector>();
                if (PlayerPrefs.GetString("Paused") == "True")
                {
                    PanelPlayableDirector.Pause();
                    if (index == 10)
                    {
                        tracingPath.SetActive(false);
                    }
                }
                else if (PlayerPrefs.GetString("Paused") == "False")
                {
                    PanelPlayableDirector.Resume();
                    PlayerPrefs.DeleteKey("Paused");
                    if (index == 10)
                    {
                        tracingPath.SetActive(true);
                    }
                }
            }
            else
            {
                gameMenu.SetActive(false);
            }
        }
    }

    void Delay2seconds(PlayableDirector director)
    {
        Invoke("HandleSwipeAnimation", 0.2f);
    }

    private IEnumerator Show_confetti()
    {
        yield return new WaitForSeconds(1.2f);
        gameobjects[6].SetActive(true);
        button[3].gameObject.SetActive(true);
    }

    private void matchedChecker()
    {
        if (total_matched == 3)
        { 
            UpdateScene();
        }
    }

    private void ResumeTimeline()
    {
        if (isPaused)
        {
            playableDirector.Play(); 
            isPaused = false;
            button[0].gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool wrong = false;

        if (other.CompareTag("Tracing Point") && !tracedPoints.Contains(other.gameObject.name))
        {
            tracedPoints.Add(other.gameObject.name);

            if (gameobjects[7].name == "Scene10")
            {
                if (wrong)
                {
                    score = 18;
                }
                
                score++;
                Debug.Log("points: " + score);

                if (other.gameObject.name == "dot1 (31)" && score >= 30)
                {
                    CheckCompletion();
                    audioManager4.Correct();
                }

                else if (other.gameObject.name == "dot1 (45)" && score >= 30)
                {
                    audioManager4.Wrong();
                    wrong = true;
                }
            }
        }
    }

    void CheckCompletion()
    {
        if (tracedPoints.Count >= totalTracingPoints)
        {
            Debug.Log("done!");
            gameobjects[11].SetActive(true);
            playableDirector.time = 0;
            playableDirector.Play();
        }
    }

    private IEnumerator CheckNailCountWithDelay()
    {
        yield return new WaitForSeconds(1f); 

        if (count_nail == 4)
        {
            transform.position = originalPosition;
            objectTransform.rotation = Quaternion.Euler(0, 0, 0);
            gameobjects[4].gameObject.SetActive(true);
            hammer[0].SetActive(false);
            hammer[1].SetActive(true);
            button[0].gameObject.SetActive(true);
        }
    }

    void HandleSwipeAnimation()
    {
        if (disinpect_step == 0)
        {
            gameobjects[6].SetActive(true);
            button[0].transform.position = treatmentItemsState;
            button[0].gameObject.SetActive(false);
            button[1].interactable = true;

            var scriptComponent1 = button[0].GetComponent<Quarter3_Level2>();
            scriptComponent1.enabled = false;
            var scriptComponent2 = button[1].GetComponent<Quarter3_Level2>();
            scriptComponent2.enabled = true;
        }
        else
        {
            button[1].transform.position = treatmentItemsState;
            button[1].gameObject.SetActive(false);
            button[2].interactable = true;

            var scriptComponent1 = button[1].GetComponent<Quarter3_Level2>();
            scriptComponent1.enabled = false;
            var scriptComponent2 = button[2].GetComponent<Quarter3_Level2>();
            scriptComponent2.enabled = true;
        }

        disinpect_step++;
        Debug.Log(disinpect_step);
    }

    private IEnumerator HandleNailHit(int index)
    {
        AnimatorStateInfo stateInfo = objectAnimation[1].GetCurrentAnimatorStateInfo(0);
        float clipLength = stateInfo.length;

        for (int i = 0; i != 2; i++)
        {
            yield return new WaitForSeconds(clipLength);
            audioManager4.SoundEffect(4);
        }

        objectAnimation[1].SetBool("martilyo", false);
        gameobjects[index].gameObject.SetActive(true);
        count_nail++;
        StartCoroutine(CheckNailCountWithDelay());
    }

    void check_cards()
    {
        StartCoroutine(FlipBackCards());
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

    private IEnumerator FlipBackCards()
    {
        foreach (var card in flippedCards)
        {
            if (isFlipping) yield break;

            isFlipping = true;
            float rotationAmount = 0f;
            Quaternion reverse_startRotation1 = card.Item1.transform.rotation;
            Quaternion reverse_endRotation1 = Quaternion.Euler(0, 0, 0);

            Quaternion reverse_startRotation2 = card.Item2.transform.rotation;
            Quaternion reverse_endRotation2 = Quaternion.Euler(0, 0, 0);

            while (rotationAmount < 1f)
            {
                rotationAmount += Time.deltaTime * rotationSpeed / 50f;
                transform.rotation = Quaternion.Slerp(reverse_startRotation1, reverse_endRotation1, rotationAmount);
                transform.rotation = Quaternion.Slerp(reverse_startRotation2, reverse_endRotation2, rotationAmount);

                if (rotationAmount >= 0.5f)
                {
                    card.Item1.gameObject.SetActive(true);
                    card.Item2.SetActive(false);

                }
                yield return null;
            }

            card.Item2.transform.rotation = reverse_endRotation1;
            card.Item1.transform.rotation = reverse_endRotation2;
            isFlipping = false;
        }

        flipCount = 0;
        flippedCards.Clear();
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
                if (flipCount > 2)
                {
                    cardbutton.gameObject.SetActive(true);
                    picture.SetActive(false);
                }
                else
                {
                    cardbutton.gameObject.SetActive(false);
                    picture.SetActive(true);
                    flipCount++;
                }
            }
            yield return null;
        }

        cardbutton.transform.rotation = endRotation1;
        picture.transform.rotation = endRotation2;
        isFlipping = false;
    }


    //------------------- 
    public List<GameObject> hammer;
    private Transform objectTransform;
    private Vector3 originalPosition;
    public List<GameObject> gameobjects;
    public GameObject hammerTip;
    private int count_nail = 0;
    public Button rotateButton;

    ////rotation -----------------
    private float rotationAngle = 90f;
    private int angleCounter = 0;
    private static int correctPuzzle = 0;
    public List<GameObject> runningWater;
    public GameObject CorrectImage;
    public int CorrectAngle = 2;
    //private int currentIndex = 0;
    //[SerializeField] private List<PlayableDirector> playableDirector2;
    [SerializeField] private PlayableDirector playableDirector;

    public void Rotate()
    {
        transform.Rotate(0f, 0f, rotationAngle);
        angleCounter++;

        if (angleCounter == CorrectAngle)
        {
            audioManager4.Correct();
            gameObject.SetActive(false);
            CorrectImage.SetActive(true);
            correctPuzzle++;
            correctAnswer_Checker();
        }
        else
        {
            audioManager4.Rotate();
        }
    }

    public void correctAnswer_Checker()
    {
        if (correctPuzzle == 3)
        {
            foreach (var Object in runningWater)
            {
                Object.SetActive(true);
            }

            gameobjects[5].SetActive(true);

            playableDirector.stopped += OnPlayableDirectorStopped;
            playableDirector.time = 0;
            playableDirector.Play();
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        button[3].gameObject.SetActive(true);
        playableDirector.stopped -= OnPlayableDirectorStopped; 
    }

    //--------------------------- falling objects
    public void Correct_object(Button obj)
    {
        obj.gameObject.SetActive(false);
        object_count++;

        if (wrong_Click > 0)
        {
            float fillamount = 0.1111111111111111f / wrong_Click;
            IncrementFillAmount(fillamount);
            wrong_Click = 0;
        }
        else
        {
            IncrementFillAmount(0.1111111111111111f);
        }

        wrong_Click = 0;

    }

    public void Wrong_object()
    {
        wrong_Click++;
    }

    public void IncrementFillAmount(float amount)
    {
        imageList[3].fillAmount = Mathf.Clamp01(imageList[3].fillAmount + amount);
    }

    private IEnumerator DelayedIncrementFillAmount()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("wrongs: " + wrong_Click);

        if (wrong_Click > 0)
        {
            float fillamount = 0.1111111111111111f / wrong_Click;
            IncrementFillAmount(fillamount);
            wrong_Click = 0;
        }
        else
        {
            IncrementFillAmount(0.1111111111111111f);
        }
    }

    public void AfterTimeline()
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

    public void DelayUpdate()
    {
        Invoke("UpdateScene", 1f);
    }
    void Show_Stars()
    {
        Debug.Log(this.gameObject);
        //NextScene_Button.gameObject.SetActive(false);

        float score = imageList[3].fillAmount * 100;
        int userID = PlayerPrefs.GetInt("Current_user");
        int theme_num = 3;
        int level_num = 2;
        int delaytime = 0;

        if (PlayerPrefs.GetFloat("Time") > 0)
        {
            StartCoroutine(requestsManager.UpdateCurrentScore("/scores", score, userID, theme_num, level_num));
        }

        if (imageList[3].fillAmount < 0.3333333333333333f)
        {
            gameobjects[18].SetActive(true);
            gameobjects[22].SetActive(false);
            gameobjects[23].SetActive(false);
            gameobjects[24].SetActive(false);
            gameobjects[25].SetActive(true);
            gameobjects[26].SetActive(false);
            delaytime = 8;
        }

        else if (imageList[3].fillAmount >= 0.3333333333333333f && imageList[3].fillAmount < 0.6666666666666667f)
        {
            gameobjects[19].SetActive(true);
            gameobjects[24].SetActive(false);
            delaytime = 12;
        }

        else if (imageList[3].fillAmount >= 0.6666666666666667f && imageList[3].fillAmount < 1f)
        {
            gameobjects[20].SetActive(true);
            delaytime = 12;
        }

        else if (Mathf.Approximately(imageList[3].fillAmount, 1f))
        {
            gameobjects[21].SetActive(true);
            delaytime = 12;
        }

        StartCoroutine(GoToMap(score, userID, delaytime));

        if (score > (100f / 3f))
        {
            StartCoroutine(requestsManager.AddReward("/reward", userID, 6));
        }
    }

    IEnumerator GoToMap(float score, int userID, int delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        if (score < (100f / 3f))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(7);
        }
        else
        {
            if (PlayerPrefs.GetFloat("Time") > 0)
            {
                int next_level = 3;
                StartCoroutine(requestsManager.UpdateCurrentLevel("/users/updateLevel", next_level, userID));
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(7);
            }
        }
    }
}
