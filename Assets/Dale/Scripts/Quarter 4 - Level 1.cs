using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Quarter4Level1 : MonoBehaviour
{
  public GameObject[] Panels;
  public GameObject GameMenu;

  [Header("<---- TIMELINE ---->")]
  [SerializeField] private TimelineAsset scene9Timelines;

  [Header("<---- PROGRESS BAR ---->")]
  [SerializeField] private GameObject bar;
  [SerializeField] private Image progressBar;

  [Header("<---- ASSESSMENT 2 GAMEOJECTS ---->")]
  [SerializeField] private GameObject[] clouds = new GameObject[5];

  [Header("<---- ASSESSMENT 2 GAMEOJECTS ---->")]
  [SerializeField] private Button[] assess2Group1 = new Button[3];
  [SerializeField] private Button[] assess2Group2 = new Button[3];

  [Header("<---- ASSESSMENT 3 GAMEOJECTS ---->")]
  [SerializeField] private Button[] assess3Group1 = new Button[5];
  [SerializeField] private Button[] assess3Group2 = new Button[5];

  [Header("<---- STARS IMAGE AND SPRITE ---->")]
  [SerializeField] private Image[] stars = new Image[3];
  [SerializeField] private Sprite earnedStar;
  [SerializeField] private GameObject[] result = new GameObject[5];

  [Header("<---- SFX CLIPS ---->")]
  [SerializeField] private AudioClip[] SFXClips;
  [SerializeField] private AudioSource SFXSource;

  [Header("<---- TRACING GAMEOBJECTS ---->")]
  [SerializeField] private GameObject[] tracingObjects;

  private int assessmentScore = 100;
  private int error = 0;
  private float score = 0;

  private void Start()
  {
    PlayerPrefs.SetInt("Tracing Points", 0);
    for (int i = 0; i < assess2Group1.Length; i++)
    {
      int index = i;
      assess2Group1[i].onClick.AddListener(() => CheckAssessment2(1, assess2Group1[index]));
    }

    for (int i = 0; i < assess2Group2.Length; i++)
    {
      int index = i;
      assess2Group2[i].onClick.AddListener(() => CheckAssessment2(2, assess2Group2[index]));
    }

    for (int i = 0; i < assess3Group1.Length; i++)
    {
      int index = i;
      assess3Group1[i].onClick.AddListener(() => CheckAssessment3(index, assess3Group1[index]));
      assess3Group2[i].onClick.AddListener(() => CheckAssessment3(index, assess3Group2[index]));
    }
  }

  public void ToggleNextPanel()
  {
    for (int i = 0; i < Panels.Length; i++)
    {
      if (Panels[i].activeInHierarchy && (i + 1) < Panels.Length)
      {
        Panels[i].SetActive(false);
        Panels[i + 1].SetActive(true);
        if (Panels[i + 1].name == "Result")
        {
          AssessResult();
        }
        break;
      }
    }
  }


  private GameObject GetActivePanel()
  {
    for (int i = 0; i < Panels.Length; i++)
    {
      if (Panels[i].activeInHierarchy) return Panels[i];
    }
    return null;
  }


  public void ToggleTimeline(TimelineAsset timeline)
  {
    PlayableDirector director = GetActivePanel().GetComponent<PlayableDirector>();
    if (timeline != null)
    {
      director.playableAsset = timeline;
      director.Play();
    }
    else
    {
      Debug.LogWarning("Timeline is not assigned.");
    }
  }

  private void Update()
  {
    GameObject currentPanel = GetActivePanel();
    string[] tracingPanels = { "Scene7", "Scene8", "Scene9", "Scene12" };

    if (tracingPanels.Contains(currentPanel.name))
    {
      if (PlayerPrefs.GetInt("Tracing Points") == 34)
      {
        PlayerPrefs.SetInt("Tracing Points", 0);
        if (currentPanel.name == "Scene9")
        {
          ToggleTimeline(scene9Timelines);
        }
        else
        {
          Invoke(nameof(ToggleNextPanel), 1.5f);
        }
      }
    }

    if (currentPanel.name == "Assessment1")
    {
      bar.SetActive(true);
      if (PlayerPrefs.HasKey("Collider"))
      {
        MoveProgress(error, 1);
        PlaySFX(SFXClips[1]);

        if (PlayerPrefs.GetString("Collider") == "final cloud")
        {
          Invoke(nameof(ToggleNextPanel), 2f);
        }

        PlayerPrefs.DeleteKey("Collider");
      }
      else if (PlayerPrefs.HasKey("Falling"))
      {
        PlaySFX(SFXClips[0]);
        PlayerPrefs.DeleteKey("Falling");
      }
    }

    if (GetActivePanel().name != "Result")
    {
      PlayableDirector playableDirector;
      playableDirector = GetActivePanel().GetComponent<PlayableDirector>();
      if (PlayerPrefs.GetString("Paused") == "True")
      {
        playableDirector.Pause();
        if (GetActivePanel().name == "Scene4")
        {
          Animator animatorBoat = GameObject.Find("ride").GetComponent<Animator>();
          Animator animatorWave = GameObject.Find("wave").GetComponent<Animator>();
          animatorBoat.speed = 0f;
          animatorWave.speed = 0f;
        }
        else if (GetActivePanel().name == "Scene7")
        {
          Animator animatorRain = GameObject.Find("scene7 rain").GetComponent<Animator>();
          AudioSource audioRain = GameObject.Find("scene7 rain").GetComponent<AudioSource>();
          animatorRain.speed = 0f;
          audioRain.Pause();
          if (tracingObjects[0].activeSelf) tracingObjects[1].SetActive(false);
        }
        else if (GetActivePanel().name == "Scene8")
        {
          if (tracingObjects[2].activeSelf) tracingObjects[3].SetActive(false);
          Animator animatorSmoke = GameObject.Find("campfire - smoking").GetComponent<Animator>();
          animatorSmoke.speed = 0f;
        }
        else if (GetActivePanel().name == "Scene9")
        {
          if (tracingObjects[4].activeSelf) tracingObjects[5].SetActive(false);
          if (playableDirector.playableAsset.name == "Scene9")
          {
            Animator animatorBurning = GameObject.Find("campfire - burning").GetComponent<Animator>();
            animatorBurning.speed = 0f;
          }
          else if (playableDirector.playableAsset.name == "Scene9 - Part2")
          {
            Animator animatorBurning = GameObject.Find("campfire - burning out").GetComponent<Animator>();
            animatorBurning.speed = 0f;
          }
        }
        else if (GetActivePanel().name == "Scene11")
        {
          GameObject worm = GameObject.Find("worm inside");
          Color wormColor = worm.GetComponent<SpriteRenderer>().color;
          wormColor.a = 0f;
          worm.GetComponent<SpriteRenderer>().color = wormColor;
        }
        else if (GetActivePanel().name == "Scene12")
        {
          if (tracingObjects[6].activeSelf) tracingObjects[7].SetActive(false);
        }
      }
      else if (PlayerPrefs.GetString("Paused") == "False")
      {
        playableDirector.Resume();
        if (GetActivePanel().name == "Scene4")
        {
          Animator animatorBoat = GameObject.Find("ride").GetComponent<Animator>();
          Animator animatorWave = GameObject.Find("wave").GetComponent<Animator>();
          animatorBoat.speed = 1f;
          animatorWave.speed = 1f;
        }
        else if (GetActivePanel().name == "Scene7")
        {
          Animator animatorRain = GameObject.Find("scene7 rain").GetComponent<Animator>();
          AudioSource audioRain = GameObject.Find("scene7 rain").GetComponent<AudioSource>();
          animatorRain.speed = 1f;
          audioRain.UnPause();
          if (tracingObjects[0].activeSelf) tracingObjects[1].SetActive(true);
        }
        else if (GetActivePanel().name == "Scene8")
        {
          if (tracingObjects[2].activeSelf) tracingObjects[3].SetActive(true);
          Animator animatorSmoke = GameObject.Find("campfire - smoking").GetComponent<Animator>();
          animatorSmoke.speed = 1f;
        }
        else if (GetActivePanel().name == "Scene9")
        {
          if (tracingObjects[4].activeSelf) tracingObjects[5].SetActive(true);
          if (playableDirector.playableAsset.name == "Scene9")
          {
            Animator animatorBurning = GameObject.Find("campfire - burning").GetComponent<Animator>();
            animatorBurning.speed = 1f;
          }
          else if (playableDirector.playableAsset.name == "Scene9 - Part2")
          {
            Animator animatorBurning = GameObject.Find("campfire - burning out").GetComponent<Animator>();
            animatorBurning.speed = 1f;
          }
        }
        else if (GetActivePanel().name == "Scene11")
        {
          GameObject worm = GameObject.Find("worm inside");
          Color wormColor = worm.GetComponent<SpriteRenderer>().color;
          wormColor.a = 1f;
          worm.GetComponent<SpriteRenderer>().color = wormColor;
        }
        else if (GetActivePanel().name == "Scene12")
        {
          if (tracingObjects[6].activeSelf) tracingObjects[7].SetActive(true);
        }
      }
    }
    else
    {
      GameMenu.SetActive(false);
    }
  }

  private void CheckAssessment2(int group, Button selectedButton)
  {
    string[] group2 = { "jump cloud", "campfire charcoal", "bottle gourd" };
    Transform[] children = selectedButton.GetComponentsInChildren<Transform>();

    if (group == 1)
    {
      string answer = selectedButton.name.Split(' ')[0];
      if (answer == "boat")
      {
        MoveProgress(error, 2);
        PlaySFX(SFXClips[1]);
        Invoke(nameof(ToggleNextPanel), 1.5f);
      }
      else
      {
        error += 25;
        PlaySFX(SFXClips[0]);
      }
    }
    else
    {
      bool correct = false;
      for (int i = 1; i < children.Length; i++) 
      {
        if (group2.Contains(children[i].name))
        {
          correct = true;
        }
        else
        {
          correct = false;
          error += 25;
          PlaySFX(SFXClips[0]);
          break;
        }
      }
      if (correct == true)
      {
        Invoke(nameof(ToggleNextPanel), 1.5f);
        MoveProgress(error, 2);
        PlaySFX(SFXClips[1]);
      }
    }
  }

  private void CheckAssessment3(int index, Button selectedButton)
  {
    string[] buttonName = selectedButton.name.Split(' ');
    Color selectedButtonColor = selectedButton.GetComponent<Image>().color;
    selectedButtonColor.a = 1;
    selectedButton.GetComponent<Image>().color = selectedButtonColor;

    Button otherButton = null;
    if (buttonName.Contains("right"))
    {
      otherButton = assess3Group1[index];
    }
    else if (buttonName.Contains("left"))
    {
      otherButton = assess3Group2[index];
    }

    MoveProgress(error, 3);
    PlaySFX(SFXClips[1]);

    Color otherButtonColor = otherButton.GetComponent<Image>().color;
    otherButtonColor.a = 1;
    otherButton.GetComponent<Image>().color = otherButtonColor;

    selectedButton.interactable = false;
    otherButton.interactable = false;

    int counter = 0;
    foreach (Button button in assess3Group1)
    {
      if (button.interactable == false) counter++;
      
    }

    if (counter == 5)
    {
      progressBar.fillAmount += 0.0000001f;
      Invoke(nameof(ToggleNextPanel), 1.5f);
    }
  }

  private void MoveProgress(int totalError, int assessNum)
  {
    float currentScore = 0;
    float finalAssessmentScore = 0;
    float scorePerGroup = 0;
    if (assessNum == 1)
    {
      scorePerGroup = assessmentScore / 5f;
    }
    else if (assessNum == 2)
    {
      scorePerGroup = assessmentScore / 2f;
    }
    else if (assessNum == 3)
    {
      scorePerGroup = assessmentScore / 5f;
    }
    finalAssessmentScore = ((float)(scorePerGroup - totalError) / assessmentScore) * (100f / 3f);

    if (finalAssessmentScore <= 0)
    {
      currentScore = 0;
    }
    else
    {
      currentScore = finalAssessmentScore;
    }

    error = 0;

    progressBar.fillAmount += currentScore / 100f;

    Debug.Log(progressBar.fillAmount);

    if (progressBar.fillAmount >= 0.3333333f && progressBar.fillAmount < 0.6666666f)
    {
      stars[0].sprite = earnedStar;
    }
    else if (progressBar.fillAmount >= 0.6666666f && progressBar.fillAmount < 0.9999999f)
    {
      stars[0].sprite = earnedStar;
      stars[1].sprite = earnedStar;
    }
    else if (progressBar.fillAmount >= 0.9999999f || progressBar.fillAmount == 1f)
    {
      stars[0].sprite = earnedStar;
      stars[1].sprite = earnedStar;
      stars[2].sprite = earnedStar;
    }
  }

  private void AssessResult()
  {
    score = progressBar.fillAmount * 100f;

    float star1 = (100f / 3f);
    float star2 = (100f / 3f) * 2;
    float star3 = (100f / 3f) * 3;

    if (score < star1)
    {
      result[4].SetActive(true);
    }
    else if (score >= star1 && score < star2)
    {
      result[1].SetActive(true);
    }
    else if (score > 99.9f || score == star3)
    {
      result[0].SetActive(true);
      result[3].SetActive(true);
    }
    else if (score >= star2 && score < star3)
    {
      result[0].SetActive(true);
      result[2].SetActive(true);
    }
  }

  public void PlaySFX(AudioClip SFXClip)
  {
    if (SFXClip != null)
    {
      SFXSource.PlayOneShot(SFXClip);
    }
    else
    {
      Debug.LogWarning("SFX clip is not assigned.");
    }
  }
}
