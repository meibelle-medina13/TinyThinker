using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Quarter4Level2 : MonoBehaviour
{
  public GameObject[] Panels;
  public GameObject GameMenu;

  [Header("<---- TIMELINE ---->")]
  [SerializeField] private GameObject[] tracingPanels;

  [Header("<---- TIMELINE ---->")]
  [SerializeField] private TimelineAsset[] scene4Timelines;
  [SerializeField] private TimelineAsset scene8Timeline;

  [Header("<---- SCENE 4 ---->")]
  [SerializeField] private GameObject shovel;
  [SerializeField] private GameObject seeds;
  [SerializeField] private GameObject sprinkler;

  [Header("<---- ASSESSMENT 1 ---->")]
  [SerializeField] private GameObject[] apples;
  [SerializeField] private GameObject appleCount;

  [Header("<---- ASSESSMENT 2 ---->")]
  [SerializeField] private GameObject[] apples2;
  [SerializeField] private GameObject[] bananas;
  [SerializeField] private GameObject[] tamarinds;
  [SerializeField] private GameObject nextButton;

  [Header("<---- STARS IMAGE AND SPRITE ---->")]
  [SerializeField] private Image[] stars = new Image[3];
  [SerializeField] private Sprite earnedStar;
  [SerializeField] private Sprite unEarnedStar;
  [SerializeField] private GameObject[] result = new GameObject[5];

  [Header("<---- PROGRESS BAR ---->")]
  [SerializeField] private GameObject bar;
  [SerializeField] private Image progressBar;

  [Header("<---- SFX CLIPS ---->")]
  [SerializeField] private AudioClip[] SFXClips;

  [Header("<---- REQUEST SCRIPT ---->")]
  [SerializeField] private THEME1_LEVEL1_REQUESTS requestsManager;

  private int assessmentScore = 100;
  private int error = 0;
  private float score = 0;
  private int userID, delaytime;

  private void Start()
  {
    PlayerPrefs.DeleteKey("Tracing Points");
  }

  public void ToggleNextPanel()
  {
    assessmentScore = 100;
    for (int i = 0; i < Panels.Length; i++)
    {
      if (Panels[i].activeInHierarchy && (i + 1) < Panels.Length)
      {
        Panels[i].SetActive(false);
        Panels[i + 1].SetActive(true);
        if (Panels[i + 1].name == "Assessment1") bar.SetActive(true);

        if (Panels[i + 1].name == "Result") AssessResult();
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
    if (currentPanel.name == "Scene4")
    {
      PlayableDirector playableDirector = currentPanel.GetComponent<PlayableDirector>();
      if (PlayerPrefs.HasKey("Collider"))
      {
        if (currentPanel.GetComponent<PlayableDirector>().playableAsset.name == "Scene4")
        {
          ToggleTimeline(scene4Timelines[0]);
        }
        else if (currentPanel.GetComponent<PlayableDirector>().playableAsset.name == "Scene4 - Part2")
        {
          ToggleTimeline(scene4Timelines[1]);
        }
        else if (currentPanel.GetComponent<PlayableDirector>().playableAsset.name == "Scene4 - Part3")
        {
          ToggleTimeline(scene4Timelines[3]);
        }
        PlayerPrefs.DeleteKey("Collider");
      }

      if (playableDirector.state == PlayState.Paused)
      {
        if (currentPanel.GetComponent<PlayableDirector>().playableAsset.name == "Scene4 - Part2" ||
        currentPanel.GetComponent<PlayableDirector>().playableAsset.name == "Scene4 - Part3")
        {
          shovel.GetComponent<BoxCollider2D>().enabled = false;
          seeds.GetComponent<BoxCollider2D>().enabled = true;
          shovel.transform.localPosition = new Vector3(0, 230, 0);
        }
        else if (currentPanel.GetComponent<PlayableDirector>().playableAsset.name == "Scene4 - Part2.1")
        {
          sprinkler.GetComponent<BoxCollider2D>().enabled = true;
          ToggleTimeline(scene4Timelines[2]);
        }
        else if (currentPanel.GetComponent<PlayableDirector>().playableAsset.name == "Scene4 - Part4")
        {
          sprinkler.transform.localPosition = new Vector3(0, -230, 0);
          sprinkler.transform.localRotation = Quaternion.Euler(0, 0, 0);
          ToggleTimeline(scene4Timelines[4]);
        }
      }
    }
    else if (currentPanel.name == "Scene8")
    {
      if (PlayerPrefs.GetInt("Tracing Points") == 34)
      {
        ToggleTimeline(scene8Timeline);
        PlayerPrefs.DeleteKey("Tracing Points");

      }
    }
    else if (currentPanel.name == "Assessment1")
    {
      if (PlayerPrefs.HasKey("Collider"))
      {
        for (int i = 0; i < apples.Length; i++)
        {
          if (!apples[i].activeSelf)
          {
            apples[i].SetActive(true);
            break;
          }
        }
        int counter = int.Parse(appleCount.GetComponent<TextMeshProUGUI>().text);
        appleCount.GetComponent<TextMeshProUGUI>().text = (counter + 1).ToString();
        if (counter + 1 <= 10)
        {
          MoveProgress(error, 1);
          PlaySFX(SFXClips[1]);
        }
        else
        {
          progressBar.fillAmount -= (3.3333333f/100f);
          PlaySFX(SFXClips[0]);
        }
        PlayerPrefs.DeleteKey("Collider");
      }
    }
    else if (currentPanel.name == "Assessment2")
    {
      if (PlayerPrefs.HasKey("Collider"))
      {
        string collider = PlayerPrefs.GetString("Collider");
        string[] trigger = PlayerPrefs.GetString("Trigger").Split(' ');
        string basketName = trigger[2];

        if (collider == basketName)
        {
          Debug.Log("Correct");
          MoveProgress(error, 2);
          PlaySFX(SFXClips[1]);
          if (basketName == "apple")
          {
            for (int i = 0; i < apples2.Length; i++)
            {
              if (!apples2[i].activeSelf)
              {
                apples2[i].SetActive(true);
                break;
              }
            }
          }
          else if (basketName == "banana")
          {
            for (int i = 0; i < bananas.Length; i++)
            {
              if (!bananas[i].activeSelf)
              {
                bananas[i].SetActive(true);
                break;
              }
            }
          }
          if (basketName == "tamarind")
          {
            for (int i = 0; i < tamarinds.Length; i++)
            {
              if (!tamarinds[i].activeSelf)
              {
                tamarinds[i].SetActive(true);
                break;
              }
            }
          }
        }
        else
        {
          Debug.Log("Wrong");
          assessmentScore -= 10;
          PlaySFX(SFXClips[0]);
        }

        PlayerPrefs.DeleteKey("Collider");
        PlayerPrefs.DeleteKey("Trigger");
      }
    }
    else if (currentPanel.name == "Assessment3")
    {
      if (PlayerPrefs.HasKey("Tracing Points"))
      {
        if (PlayerPrefs.GetInt("Tracing Points") == 34)
        {
          progressBar.fillAmount += 0.0000001f;
          PlaySFX(SFXClips[1]);
          Invoke(nameof(ToggleNextPanel), 1.5f);
        }
        MoveProgress(error, 3);
        PlayerPrefs.DeleteKey("Tracing Points");
      }
    }

    if (progressBar.fillAmount >= 0.3333333f && progressBar.fillAmount < 0.666666f)
    {
      stars[0].sprite = earnedStar;
    }
    else if (progressBar.fillAmount >= 0.666666f && progressBar.fillAmount < 0.999999f)
    {
      stars[0].sprite = earnedStar;
      stars[1].sprite = earnedStar;
    }
    else if (progressBar.fillAmount >= 0.999999f || progressBar.fillAmount == 1f)
    {
      stars[0].sprite = earnedStar;
      stars[1].sprite = earnedStar;
      stars[2].sprite = earnedStar;
    }
    else
    {
      stars[0].sprite = unEarnedStar;
      stars[1].sprite = unEarnedStar;
      stars[2].sprite = unEarnedStar;
    }

    if (GetActivePanel().name != "Result")
    {
      PlayableDirector playableDirector;
      playableDirector = GetActivePanel().GetComponent<PlayableDirector>();
      if (PlayerPrefs.GetString("Paused") == "True")
      {
        playableDirector.Pause();
        if (currentPanel.name == "Scene8")
        {
          if (tracingPanels[0].activeSelf)
          {
            tracingPanels[1].SetActive(false);
          }
        }
        else if (currentPanel.name == "Assessment3")
        {
          if (tracingPanels[2].activeSelf)
          {
            tracingPanels[3].SetActive(false);
          }
        }
      }
      else if (PlayerPrefs.GetString("Paused") == "False")
      {
        playableDirector.Resume();
        if (currentPanel.name == "Scene8")
        {
          if (tracingPanels[0].activeSelf)
          {
            tracingPanels[1].SetActive(true);
          }
        }
        else if (currentPanel.name == "Assessment3")
        {
          if (tracingPanels[2].activeSelf)
          {
            tracingPanels[3].SetActive(true);
          }
        }
      }
    }
    else
    {
      GameMenu.SetActive(false);
    }
  }

  public void DragObject(GameObject obj)
  {
    obj.transform.position = Input.mousePosition;
    obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, 0f);
  }

  private void MoveProgress(int totalError, int assessNum)
  {
    float currentScore = 0;
    float finalAssessmentScore = 0;
    float scorePerGroup = 0;
    if (assessNum == 1)
    {
      scorePerGroup = assessmentScore / 10f;
    }
    else if (assessNum == 2)
    {
      scorePerGroup = assessmentScore / 6f;
    }
    else if (assessNum == 3)
    {
      scorePerGroup = assessmentScore / 34f;
    }
    finalAssessmentScore = ((float)(scorePerGroup - totalError) / 100f) * (100f / 3f);

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
  }

  private void AssessResult()
  {

    int theme_num = 4;
    int level_num = 2;
    userID = PlayerPrefs.GetInt("Current_user");

    score = progressBar.fillAmount * 100f;

    if (PlayerPrefs.GetFloat(userID.ToString() + "Time") > 0)
    {
      StartCoroutine(requestsManager.UpdateCurrentScore("/scores", score, userID, theme_num, level_num));
    }

    float star1 = (100f / 3f);
    float star2 = (100f / 3f) * 2;
    float star3 = (100f / 3f) * 3;

    if (score < star1)
    {
      result[4].SetActive(true);
      delaytime = 8;
    }
    else if (score >= star1 && score < star2)
    {
      result[1].SetActive(true);
      delaytime = 12;
    }
    else if (score > 99.9f || score == star3)
    {
      result[0].SetActive(true);
      result[3].SetActive(true);
      delaytime = 20;
    }
    else if (score >= star2 && score < star3)
    {
      result[0].SetActive(true);
      result[2].SetActive(true);
      delaytime = 12;
    }

    StartCoroutine(GoToMap());

    if (score > (100f / 3f))
    {
      StartCoroutine(requestsManager.AddReward("/reward", userID, 8));
    }
  }

  IEnumerator GoToMap()
  {
    yield return new WaitForSeconds(delaytime);
    if (score < (100f / 3f))
    {
      UnityEngine.SceneManagement.SceneManager.LoadScene(7);
    }
    else
    {
      if (PlayerPrefs.GetFloat(userID.ToString() + "Time") > 0)
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


  [SerializeField] private AudioSource SFXSource;

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
