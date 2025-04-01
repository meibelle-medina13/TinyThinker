using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Quarter2Level5 : MonoBehaviour
{
  // SWITCHING PANELS
  // -------------------------------------------------- //
  public GameObject[] Panels;

  [Header("<---- GAME MENU ---->")]
  [SerializeField] private GameObject gameMenu;

  [Header("<---- TRACING OBJECTS ---->")]
  [SerializeField] private GameObject[] tracingObject;

  void Start()
  {
    Panels[0].SetActive(true);
  }


  public GameObject Pencil1;
  public GameObject PencilMask1;
  public GameObject Collider1;
  private Vector3 pencilState1;
  private Vector3 pencilRaise1 = new Vector3(105, 120, 0);
  private Vector3 pencilWrite1 = new Vector3(85, 100, 0);

  public GameObject Pencil2;
  public GameObject PencilMask2;
  public GameObject Collider2;
  private Vector3 pencilState2;
  private Vector3 pencilRaise2 = new Vector3(105, 120, 0);
  private Vector3 pencilWrite2 = new Vector3(85, 100, 0);

  public GameObject Pencil3;
  public GameObject PencilMask3;
  public GameObject Collider3;
  private Vector3 pencilState3;
  private Vector3 pencilRaise3 = new Vector3(105, 120, 0);
  private Vector3 pencilWrite3 = new Vector3(85, 100, 0);

  void Update()
  {
    if (GetActivePanel() == "Scene7")
    {
      Vector3 screenPosition = Input.mousePosition;
      screenPosition.z = Camera.main.nearClipPlane + 1;
      Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

      if (Input.GetMouseButton(0))
      {
        Collider3.GetComponent<CircleCollider2D>().enabled = true;
        pencilState3 = pencilWrite3;

        GameObject pencilMask = Instantiate(PencilMask3, worldPosition, Quaternion.identity);
        pencilMask.transform.SetParent(Panels[7].transform);
      }
      else
      {
        Collider3.GetComponent<CircleCollider2D>().enabled = false;
        pencilState3 = pencilRaise3;
      }
      Pencil3.transform.position = worldPosition + pencilState3;
      Collider3.transform.position = worldPosition;
    }
    else if (GetActivePanel() == "Assessment2")
    {
      Vector3 screenPosition = Input.mousePosition;
      screenPosition.z = Camera.main.nearClipPlane + 1;
      Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

      if (Input.GetMouseButton(0))
      {
        Collider1.GetComponent<CircleCollider2D>().enabled = true;
        pencilState1 = pencilWrite1;

        GameObject pencilMask = Instantiate(PencilMask1, worldPosition, Quaternion.identity);
        pencilMask.transform.SetParent(Panels[0].transform);
      }
      else
      {
        Collider1.GetComponent<CircleCollider2D>().enabled = false;
        pencilState1 = pencilRaise1;
      }
      Pencil1.transform.position = worldPosition + pencilState1;
      Collider1.transform.position = worldPosition;
    }
    else if (GetActivePanel() == "Assessment3")
    {
      Vector3 screenPosition = Input.mousePosition;
      screenPosition.z = Camera.main.nearClipPlane + 1;
      Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

      if (Input.GetMouseButton(0))
      {
        Collider2.GetComponent<CircleCollider2D>().enabled = true;
        pencilState2 = pencilWrite2;

        GameObject pencilMask = Instantiate(PencilMask2, worldPosition, Quaternion.identity);
        pencilMask.transform.SetParent(Panels[0].transform);
      }
      else
      {
        Collider2.GetComponent<CircleCollider2D>().enabled = false;
        pencilState2 = pencilRaise2;
      }
      Pencil2.transform.position = worldPosition + pencilState2;
      Collider2.transform.position = worldPosition;
    }

    int index = 0;

    if (gameObject.name == "Scene Manager")
    {
      for (int i = 0; i < Panels.Length; i++)
      {
        if (Panels[i].activeSelf)
        {
          index = i;
        }
      }

      PlayableDirector playableDirector;

      if (index != 18 && index != 16 && index != 17)
      {
        //Debug.Log("Panel" + index);

        playableDirector = Panels[index].GetComponent<PlayableDirector>();

        if (PlayerPrefs.GetString("Paused") == "True")
        {
          playableDirector.Pause();
        }
        else
        {
          playableDirector.Resume();
        }
      }
      else
      {
        if (index == 18)
        {
          PlayerPrefs.SetFloat("Theme2 Score", totalProgressFill);
          gameMenu.SetActive(false);
        }
        else
        {
          if (PlayerPrefs.GetString("Paused") == "True")
          {
            if (index == 16)
            {
              tracingObject[0].SetActive(false);
            }
            else if (index == 17)
            {
              tracingObject[1].SetActive(false);
            }
          }
          else
          {
            if (index == 16)
            {
              tracingObject[0].SetActive(true);
            }
            else if (index == 17)
            {
              tracingObject[1].SetActive(true);
            }
          }
        }
        
      }

    }
  }


  public void TogglePanel()
  {
    for (int i = 0; i < Panels.Length; i++)
    {
      if (Panels[i].activeInHierarchy && (i + 1) < Panels.Length)
      {
        Panels[i].SetActive(false);
        Panels[i + 1].SetActive(true);

        if (Panels[i + 1].transform.name == "Assessment1.1" || Panels[i + 1].transform.name == "Result")
        ToggleProgressBar();
        break;
      }
    }
  }


  private string GetActivePanel()
  {
    for (int i = 0; i < Panels.Length; i++)
    {
      if (Panels[i].activeInHierarchy) return Panels[i].transform.name;
    }
    return "None";
  }


  public GameObject ProgressBar;

  private void ToggleProgressBar()
  {
    ProgressBar.SetActive(!ProgressBar.activeInHierarchy);
  }


  public GameObject Result;
  public GameObject ZeroStar;
  public GameObject OneStar;
  public GameObject TwoStars;
  public GameObject ThreeStars;
  static int starCount;

  private void ToggleResult()
  {
    Panels[0].SetActive(!Panels[0].activeInHierarchy);
    Result.SetActive(!Result.activeInHierarchy);

    if (starCount == 0)
    {
      ZeroStar.SetActive(true);
      PlayerPrefs.SetInt("Delay Time", 7);
    }
    else if (starCount == 1)
    {
      OneStar.SetActive(true);
      PlayerPrefs.SetInt("Delay Time", 8);
    }
    else if (starCount == 2)
    {
      TwoStars.SetActive(true);
      PlayerPrefs.SetInt("Delay Time", 8);
    }
    else if (starCount == 3)
    {
      ThreeStars.SetActive(true);
      PlayerPrefs.SetInt("Delay Time", 14);
    }
    ProgressBar.SetActive(false);
  }
  // -------------------------------------------------- //


  // GAMING COMMANDS
  // -------------------------------------------------- //
  public void ChoiceA()
  {
    if (GetActivePanel() == "Assessment1.1")
    {
      Debug.Log("Correct");
      
      AddPoints();
    }
    else if (GetActivePanel() == "Assessment1.2")
    {
      Debug.Log("Wrong");
      SubPoints();
    }
  }


  public void ChoiceB()
  {
    if (GetActivePanel() == "Assessment1.1")
    {
      Debug.Log("Wrong");
      SubPoints();
    }
    else if (GetActivePanel() == "Assessment1.2")
    {
      Debug.Log("Correct");
      AddPoints();
    }
  }


  public void ChoiceC()
  {
    if (GetActivePanel() == "Assessment1.1")
    {
      Debug.Log("Wrong");
      SubPoints();
    }
    else if (GetActivePanel() == "Assessment1.2")
    {
      Debug.Log("Wrong");
      SubPoints();
    }
  }


  HashSet<string> tracedPoints1 = new HashSet<string>();
  HashSet<string> tracedPoints2 = new HashSet<string>();
  HashSet<string> tracedPoints3 = new HashSet<string>();

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (GetActivePanel() == "Assessment2" && collider.CompareTag("Tracing Point") && !tracedPoints1.Contains(collider.gameObject.name))
    {
      Debug.Log(collider.gameObject.name);
      tracedPoints1.Add(collider.gameObject.name);
      AddPoints();
    }
    else if (GetActivePanel() == "Assessment3" && collider.CompareTag("Tracing Point") && !tracedPoints2.Contains(collider.gameObject.name))
    {
      Debug.Log(collider.gameObject.name);
      tracedPoints2.Add(collider.gameObject.name);
      AddPoints();
    }
  }
  // -------------------------------------------------- //

  // SCORING
  // -------------------------------------------------- //
  static float starFillMultiplier = .33333f;

  static float assessment1_1 = 50f;
  static float a1_1_ProgressFill;
  static float a1_1_Points;
  static float a1_1_Mistakes;

  static float assessment1_2 = 50f;
  static float a1_2_ProgressFill;
  static float a1_2_Points;
  static float a1_2_Mistakes;

  static float assessment2 = 100f;
  static float a2_ProgressFill;
  static float a2_Points;
  static float a2_TracingPoints = 39f;

  static float assessment3 = 100f;
  static float a3_ProgressFill;
  static float a3_Points;
  static float a3_TracingPoints = 22f;

  private void AddPoints()
  {
    if (GetActivePanel() == "Assessment1.1")
    {
      a1_1_Points += assessment1_1;
      Debug.Log("A1.1: " + a1_1_Points);

      a1_1_ProgressFill = (a1_1_Points / 100f) * starFillMultiplier;

      PlaySFX("correct");
      TogglePanel();
      OnProgress();
    }
    else if (GetActivePanel() == "Assessment1.2")
    {
      a1_2_Points += assessment1_2;
      Debug.Log("A1.2: " + a1_2_Points);

      a1_2_ProgressFill = (a1_2_Points / 100f) * starFillMultiplier;

      PlaySFX("correct");
      TogglePanel();
      OnProgress();
    }
    else if (GetActivePanel() == "Assessment2")
    {
      if (a2_TracingPoints == tracedPoints1.Count)
      {
        a2_Points += (assessment2 / a2_TracingPoints) + 0.00001f; // ensures 100 score
        Debug.Log("A2: " + a2_Points);

        a2_ProgressFill = (a2_Points / 100f) * starFillMultiplier;
        TogglePanel();
      }
      else
      {
        a2_Points += assessment2 / a2_TracingPoints;
        Debug.Log("A2: " + a2_Points);

        a2_ProgressFill = (a2_Points / 100f) * starFillMultiplier;
      }
      OnProgress();
    }
    else if (GetActivePanel() == "Assessment3")
    {
      if (a3_TracingPoints == tracedPoints2.Count)
      {
        a3_Points += (assessment3 / a3_TracingPoints);
        Debug.Log("A3: " + a3_Points);

        a3_ProgressFill = ((a3_Points / 100f) * starFillMultiplier) + 0.0000099f; // ensure 100 score
        OnProgress();
        ToggleResult();
      }
      else
      {
        a3_Points += assessment3 / a3_TracingPoints;
        Debug.Log("A3: " + a3_Points);

        a3_ProgressFill = (a3_Points / 100f) * starFillMultiplier;
        OnProgress();
      }
      
    }
  }


  private void SubPoints()
  {
    if (GetActivePanel() == "Assessment1.1" && a1_1_Mistakes < 2f)
    {
      a1_1_Mistakes++;
      a1_1_Points -= (a1_1_Mistakes / 3f) * assessment1_1;
      Debug.Log("A1.1: " + a1_1_Points);

      a1_1_ProgressFill = (a1_1_Points / 100f) * starFillMultiplier;
      OnProgress();
    }
    else if (GetActivePanel() == "Assessment1.2" && a1_2_Mistakes < 2f)
    {
      a1_2_Mistakes++;
      a1_2_Points -= (a1_2_Mistakes / 3f) * assessment1_2;
      Debug.Log("A1.2: " + a1_2_Points);

      a1_2_ProgressFill = (a1_2_Points / 100f) * starFillMultiplier;
      OnProgress();
    }
    PlaySFX("wrong");
  }


  public Image ProgressBarMask;
  public Sprite EarnedStar;
  public Image[] UnearnedStarImages;

  static float totalProgressFill;

  private void OnProgress()
  {
    totalProgressFill = a1_1_ProgressFill + a1_2_ProgressFill + a2_ProgressFill + a3_ProgressFill;

    ProgressBarMask.fillAmount = totalProgressFill;

    if (totalProgressFill == 1f)
    {
      UnearnedStarImages[0].sprite = EarnedStar;
      UnearnedStarImages[1].sprite = EarnedStar;
      UnearnedStarImages[2].sprite = EarnedStar;
      starCount = 3;
      Debug.Log("Stars: " + starCount);
    }
    else if (totalProgressFill >= .66666f && totalProgressFill < 1)
    {
      UnearnedStarImages[0].sprite = EarnedStar;
      UnearnedStarImages[1].sprite = EarnedStar;
      starCount = 2;
      Debug.Log("Stars: " + starCount);
    }
    else if (totalProgressFill >= .33333f && totalProgressFill < .66666f)
    {
      UnearnedStarImages[0].sprite = EarnedStar;
      starCount = 1;
      Debug.Log("Stars: " + starCount);
    }
  }
  // ------------------------------------------------------------------------------------------------ //

  // -------------------------------------------------- //

  // PLAYING BGM AND SFX
  // -------------------------------------------------- //
  [SerializeField] public AudioSource audioSource;
  [SerializeField] public AudioClip button, correct, wrong, maglinis, maglaro, matulog, linis, hugas, kumpuni;

  public void PlaySFX(string sfx)
  {
    if (sfx == "button") audioSource.PlayOneShot(button);
    else if (sfx == "correct") audioSource.PlayOneShot(correct);
    else if (sfx == "wrong") audioSource.PlayOneShot(wrong);
    else if (sfx == "maglinis") audioSource.PlayOneShot(maglinis);
    else if (sfx == "maglaro") audioSource.PlayOneShot(maglaro);
    else if (sfx == "matulog") audioSource.PlayOneShot(matulog);
    else if (sfx == "linis") audioSource.PlayOneShot(linis);
    else if (sfx == "hugas") audioSource.PlayOneShot(hugas);
    else if (sfx == "kumpuni") audioSource.PlayOneShot(kumpuni);
  }
  // -------------------------------------------------- //
}
