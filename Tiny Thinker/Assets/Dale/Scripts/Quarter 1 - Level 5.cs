using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class Quarter1Level5 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
  // SWITCHING PANELS
  // -------------------------------------------------- //
  public GameObject[] Panels;

  void Start()
  {
    Panels[0].SetActive(true);
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
  // -------------------------------------------------- //


  // GAMING COMMANDS
  // -------------------------------------------------- //
  private Vector3 offset;
  public GameObject Shape;
  public GameObject Pencil;
  public GameObject PencilMaskPrefab;
  bool isDragging, isDropped, isDrawing;

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

      Pencil.transform.position = worldPosition;

      if (!isDrawing) return;
      offset = new Vector3(110, 130, 0);
      GameObject pencilMask = Instantiate(PencilMaskPrefab, (worldPosition - offset), Quaternion.identity);
      pencilMask.transform.SetParent(Panels[0].transform);
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

      ShapeAudioSource.PlayOneShot(ShapeSFX);
    }
    else if (Panels[0].transform.name == "Assessment2")
    {
      isDrawing = true;
      Pencil.GetComponent<CircleCollider2D>().enabled = true;
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
          ShapeAudioSource.PlayOneShot(Correct);
          return;
        }
        else if (
          Shape.transform.name != Regex.Replace(Slots[i].transform.name, @"\s.*", "") &&
          Vector2.Distance(Shape.transform.position, Slots[i].transform.position) < 100
        )
        {
          SubPoints();
          ShapeAudioSource.PlayOneShot(Wrong);
        }
      }

      isDragging = false;
      Shape.transform.position = initialPosition;
      Shape.transform.SetSiblingIndex(siblingIndex);
      ShapeImage.raycastTarget = true;
    }
    else if (Panels[0].transform.name == "Assessment2")
    {
      isDrawing = false;
      Pencil.GetComponent<CircleCollider2D>().enabled = false;
    }
  }


  HashSet<string> tracedPoints = new HashSet<string>();

  public void OnTriggerEnter2D(Collider2D collider2D)
  {
    if (collider2D.CompareTag("Tracing Point") && !tracedPoints.Contains(collider2D.gameObject.name))
    {
      tracedPoints.Add(collider2D.gameObject.name);
      AddPoints();
    }
  }
  // -------------------------------------------------- //


  // SCORING
  // -------------------------------------------------- //
  static float starFillMultiplier;

  static float assessment1 = 100;
  static float a1_ProgressFill;
  static float a1_Points;
  static float a1_VacantSlots = 4;

  static float assessment2 = 100;
  static float a2_ProgressFill;
  static float a2_Points;
  static float a2_TracingPoints = 19;

  static float assessment3 = 100;
  static float a3_ProgressFill;
  static float a3_Points;
  static float a3_VacantSlots = 4;

  private void AddPoints()
  {
    if (Panels[0].transform.name == "Assessment1")
    {
      starFillMultiplier = .5f;

      a1_Points += assessment1 / Slots.Length;
      Debug.Log("A1: " + a1_Points);

      a1_ProgressFill = (a1_Points / 100) * starFillMultiplier;
      a1_VacantSlots--;

      if (a1_VacantSlots == 0) TogglePanel();
    }
    else if (Panels[0].transform.name == "Assessment2")
    {
      starFillMultiplier = .25f;
      if (a1_ProgressFill == .0f) starFillMultiplier = .5f;

      a2_Points += assessment2 / a2_TracingPoints;
      Debug.Log("A2: " + a2_Points);

      a2_ProgressFill = (a2_Points / 100) * starFillMultiplier;

      if (a2_Points >= 100f) TogglePanel(); // no other trigger for now
    }
    else if (Panels[0].transform.name == "Assessment3")
    {
      starFillMultiplier = .25f;
      if (a1_ProgressFill == .0f && a2_ProgressFill == .0f) starFillMultiplier = .5f;

      a3_Points += assessment3 / Slots.Length;
      Debug.Log("A3: " + a3_Points);

      a3_ProgressFill = (a3_Points / 100) * starFillMultiplier;
      a3_VacantSlots--;

      if (a3_VacantSlots == 0) TogglePanel();
    }
    OnProgress();
  }

  private void SubPoints()
  {
    if (Panels[0].transform.name == "Assessment1")
    {
      starFillMultiplier = .5f;

      a1_Points -= (assessment1 / Slots.Length) / a1_VacantSlots;
      a1_ProgressFill = (a1_Points / assessment1) * starFillMultiplier;
      Debug.Log("A1: " + a1_Points);
    }
    else if (Panels[0].transform.name == "Assessment2")
    {
      // empty
    }
    else if (Panels[0].transform.name == "Assessment3")
    {
      starFillMultiplier = .25f;
      if (a1_ProgressFill == .0f && a2_ProgressFill == .0f) starFillMultiplier = .5f;

      a3_Points -= (assessment3 / Slots.Length) / a3_VacantSlots;
      a3_ProgressFill = (a3_Points / assessment3) * starFillMultiplier;
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

    if (totalProgressFill == 1)
    {
      UnearnedStarImages[0].sprite = EarnedStar;
      UnearnedStarImages[1].sprite = EarnedStar;
      UnearnedStarImages[2].sprite = EarnedStar;
    }
    else if (totalProgressFill >= .75F && totalProgressFill < 1)
    {
      UnearnedStarImages[0].sprite = EarnedStar;
      UnearnedStarImages[1].sprite = EarnedStar;
    }
    else if (totalProgressFill >= .5F && totalProgressFill < .75F)
    {
      UnearnedStarImages[0].sprite = EarnedStar;
    }
  }
  // -------------------------------------------------- //


  // PLAYING BGM AND SFX
  // -------------------------------------------------- //
  public void PlaySFX(AudioSource audioSource, AudioClip SFX)
  {
    if (audioSource == null && SFX == null) return;
    audioSource.PlayOneShot(SFX);
  }
  // -------------------------------------------------- //
}
