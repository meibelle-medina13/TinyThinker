using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class Quarter1Level5 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
  // SWITCHING PANELS
  // -------------------------------------------------- //
  public GameObject[] Panels;
  [SerializeField] private AudioSource LevelAudioSource;
  [SerializeField] private AudioClip LevelBGM;

  void Start()
  {
    Panels[0].SetActive(true);
    PlayBGM(LevelAudioSource, LevelBGM);
  }


  public void TogglePanel()
  {
    for (int i = 0; i < Panels.Length; i++)
    {
      if (Panels[i].activeInHierarchy && (i + 1) < Panels.Length)
      {
        Panels[i].SetActive(false);
        Panels[i + 1].SetActive(true);

        if (
          Panels[i + 1].transform.name == "Assessment1" ||
          Panels[i + 1].transform.name == "Result"
        ) 
        ToggleProgressBar();
        break;
      }

    }
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
    }
    else if (starCount == 1)
    {
      OneStar.SetActive(true);
      PlayerPrefs.SetInt("Delay Time", 4);
    }
    else if (starCount == 2)
    {
      TwoStars.SetActive(true);
        PlayerPrefs.SetInt("Delay Time", 4);
    }
    else if (starCount == 3)
    {
      ThreeStars.SetActive(true);
        PlayerPrefs.SetInt("Delay Time", 4);
    }

  }
  // -------------------------------------------------- //


  // GAMING COMMANDS
  // -------------------------------------------------- //
  private Vector3 offset;
  public GameObject Shape;
  public GameObject Pencil;
  public GameObject PencilMask;
  public GameObject Collider;
  private Vector3 pencilState;
  private Vector3 pencilRaise = new Vector3(105, 120, 0);
  private Vector3 pencilWrite = new Vector3(85, 100, 0);
  bool isDragging, isDropped;

  void Update()
  {
    if (Panels[0].transform.name == "Assessment1" || Panels[0].transform.name == "Assessment3")
    {
      if (!isDragging || isDropped) return;
      Shape.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
    }
    else if (Panels[0].transform.name == "Assessment2")
    {
      Vector3 screenPosition = Input.mousePosition;
      screenPosition.z = Camera.main.nearClipPlane + 1;
      Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

      if (Input.GetMouseButton(0))
      {
        Collider.GetComponent<CircleCollider2D>().enabled = true;
        pencilState = pencilWrite;

        GameObject pencilMask = Instantiate(PencilMask, worldPosition, Quaternion.identity);
        pencilMask.transform.SetParent(Panels[0].transform);
      }
      else
      {
        Collider.GetComponent<CircleCollider2D>().enabled = false;
        pencilState = pencilRaise;
      }

      Pencil.transform.position = worldPosition + pencilState;
      Collider.transform.position = worldPosition;
    }
  }


  [SerializeField] private AudioSource ShapeAudioSource;
  [SerializeField] private AudioClip ShapeSFX, Correct, Wrong;
  private Vector3 initialPosition;
  public Image ShapeImage;
  int siblingIndex;

  public void OnPointerDown(PointerEventData eventData)
  {
    if (Panels[0].transform.name == "Assessment1" || Panels[0].transform.name == "Assessment3")
    {
      isDragging = true;
      initialPosition = Shape.transform.position;
      offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Shape.transform.position;
      siblingIndex = Shape.transform.GetSiblingIndex();
      Shape.transform.SetAsLastSibling();
      ShapeImage.raycastTarget = false;

      PlaySFX(ShapeAudioSource, ShapeSFX);
    }
  }


  public GameObject[] Slots;

  public void OnPointerUp(PointerEventData eventData)
  {
    if (Panels[0].transform.name == "Assessment1" || Panels[0].transform.name == "Assessment3")
    {
      for (int i = 0; i < Slots.Length; i++)
      {
        if (
          Shape.transform.name == Regex.Replace(Slots[i].transform.name, @"\s.*", "") &&
          Vector2.Distance(Shape.transform.position, Slots[i].transform.position) < 100
        )
        {
          isDropped = true;
          Shape.transform.position = Slots[i].transform.position;

          AddPoints();
          PlaySFX(ShapeAudioSource, Correct);
          return;
        }
        else if (
          Shape.transform.name != Regex.Replace(Slots[i].transform.name, @"\s.*", "") &&
          Vector2.Distance(Shape.transform.position, Slots[i].transform.position) < 100
        )
        {
          SubPoints();
          PlaySFX(ShapeAudioSource, Wrong);
        }
      }

      isDragging = false;
      Shape.transform.position = initialPosition;
      Shape.transform.SetSiblingIndex(siblingIndex);
      ShapeImage.raycastTarget = true;
    }
  }


  HashSet<string> tracedPoints = new HashSet<string>();

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.CompareTag("Tracing Point") && !tracedPoints.Contains(collider.gameObject.name))
    {
      tracedPoints.Add(collider.gameObject.name);
      AddPoints();
    }
  }
  // -------------------------------------------------- //


  // SCORING
  // -------------------------------------------------- //
  static float starFillMultiplier = .33333f;

  static float assessment1 = 100f;
  static float a1_ProgressFill;
  static float a1_Points;
  static float a1_VacantSlots = 4f;

  static float assessment2 = 100f;
  static float a2_ProgressFill;
  static float a2_Points;
  static float a2_TracingPoints = 28f;

  static float assessment3 = 100f;
  static float a3_ProgressFill;
  static float a3_Points;
  static float a3_VacantSlots = 4f;

  private void AddPoints()
  {
    if (Panels[0].transform.name == "Assessment1")
    {
      a1_Points += assessment1 / Slots.Length;
      Debug.Log("A1: " + a1_Points);

      a1_ProgressFill = (a1_Points / 100f) * starFillMultiplier;
      a1_VacantSlots--;

      if (a1_VacantSlots == 0) TogglePanel();
    }
    else if (Panels[0].transform.name == "Assessment2")
    {
      a2_Points += assessment2 / a2_TracingPoints;
      Debug.Log("A2: " + a2_Points);

      a2_ProgressFill = (a2_Points / 100f) * starFillMultiplier; // not 100

      if (a2_TracingPoints == tracedPoints.Count) TogglePanel();
    }
    else if (Panels[0].transform.name == "Assessment3")
    {
      a3_Points += assessment3 / Slots.Length;
      Debug.Log("A3: " + a3_Points);

      a3_ProgressFill = (a3_Points / 100f) * starFillMultiplier;
      a3_VacantSlots--;

      if (a3_VacantSlots == 0) ToggleResult();
    }
    OnProgress();
  }

  private void SubPoints()
  {
    if (Panels[0].transform.name == "Assessment1")
    {
      a1_Points -= (assessment1 / Slots.Length) / a1_VacantSlots;
      a1_ProgressFill = (a1_Points / 100f) * starFillMultiplier;
      Debug.Log("A1: " + a1_Points);
    }
    else if (Panels[0].transform.name == "Assessment3")
    {
      a3_Points -= (assessment3 / Slots.Length) / a3_VacantSlots;
      a3_ProgressFill = (a3_Points / 100f) * starFillMultiplier;
      Debug.Log("A3: " + a3_Points);
    }
    OnProgress();
  }


  public Image ProgressBarMask;
  public Sprite EarnedStar;
  public Image[] UnearnedStarImages;

  static float totalProgressFill;

  private void OnProgress()
  {
    totalProgressFill = a1_ProgressFill + a2_ProgressFill + a3_ProgressFill;

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
    // ------------------------------------------------------------------------------------------------ //
    PlayerPrefs.SetFloat("Level5 Score", totalProgressFill);
    }
  // -------------------------------------------------- //


  // PLAYING BGM AND SFX
  // -------------------------------------------------- //
  public void PlayBGM(AudioSource audioSource, AudioClip BGM)
  {
    if (audioSource == null && BGM == null) return;
    audioSource.clip = BGM;
    audioSource.Play();
  }

  public void PlaySFX(AudioSource audioSource, AudioClip SFX)
  {
    if (audioSource == null && SFX == null) return;
    audioSource.PlayOneShot(SFX);
  }
    // -------------------------------------------------- //
}
