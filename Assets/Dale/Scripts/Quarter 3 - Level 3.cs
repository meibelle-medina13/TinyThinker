using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class Quarter3Level3 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
  // SWITCHING PANELS
  // -------------------------------------------------- //
  public GameObject[] Panels;
  public GameObject tracingObject;
  public GameObject assess2BG;
  public GameObject[] moleHole;

  void Start()
  {
    Panels[0].SetActive(true);
    if (gameObject.name == "Scene Manager")
    {
      a1_ProgressFill = 0;
      a1_Points = 0;
      a1_PlayerScore = 0;
      a1_PlayerMistakes = 0;

      a2_ProgressFill = 0;
      a2_Points = 0;
      a2_PlayerScore = 0;

      a3_ProgressFill = 0;
      a3_Points = 0;
      a3_PlayerScore = 0;
      a3_PlayerMistakes = 0;
    }
  }


  // Lesson
  private GameObject pencil;
  public GameObject PencilMask;
  private Vector3 pencilState;
  private Vector3 pencilRaise = new Vector3(105, 120, 0);
  private Vector3 pencilWrite = new Vector3(85, 100, 0);

  // Assessment 1
  bool isDragging, isDropped;
  bool isHammering = false;
  private Vector3 offset;
  public GameObject schoolMaterial;
  private Vector3 schoolMaterialInitialPosition;
  public Image schoolMaterialImage;
  int siblingIndex;

  // Assessment 2
  public GameObject WhackAHammer;
  public Sprite WhackABammerSprite;
  public Sprite WhackAHammerSprite;
  public GameObject WhackACollider;
  private Vector3 WhackAHammerState;

  void Update()
  {
    if (GetActivePanel() == "Scene4")
    {
      if (pencil != null)
      {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane + 1;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (Input.GetMouseButton(0))
        {
          pencilState = pencilWrite;

          GameObject pencilMask = Instantiate(PencilMask, worldPosition, Quaternion.identity);
          pencilMask.transform.SetParent(Panels[4].transform);
        }
        else
        {
          pencilState = pencilRaise;
        }
        pencil.transform.position = worldPosition + pencilState;
      }
      else
      {
        Debug.LogWarning("Missing GameObject: pencil!");
      }
    }
    else if (GetActivePanel() == "Assessment1")
    {
      if (!isDragging || isDropped) return;
      schoolMaterial.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
    }
    else if (GetActivePanel() == "Assessment2")
    {
      Vector3 screenPosition = Input.mousePosition;
      screenPosition.z = Camera.main.nearClipPlane + 1;
      Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

      if (Input.GetMouseButton(0))
      {
        WhackACollider.GetComponent<CircleCollider2D>().enabled = true;
        WhackAHammer.GetComponent<Image>().sprite = WhackABammerSprite;

        if (!isHammering)
        {
          isHammering = true;
          PlaySFX("cartoon hammer");
        }
      }
      else
      {
        WhackACollider.GetComponent<CircleCollider2D>().enabled = false;
        WhackAHammer.GetComponent<Image>().sprite = WhackAHammerSprite;
        isHammering = false;
      }

      WhackAHammer.transform.position = worldPosition;
      WhackACollider.transform.position = worldPosition + new Vector3(-10, -30, 0);
    }

    if (GetActivePanel() != "Result")
    {
      PlayableDirector playableDirector;
      int index = PlayerPrefs.GetInt("CurrentPanel");
      playableDirector = Panels[index].GetComponent<PlayableDirector>();

      if (PlayerPrefs.GetString("Paused") == "True")
      {
        playableDirector.Pause();
        if (GetActivePanel() == "Scene4") tracingObject.SetActive(false);

        if (GetActivePanel() == "Assessment2")
        {
          if (moleHole.Length > 0)
          {
            foreach (GameObject hole in moleHole)
            {
              hole.SetActive(false);
            }
          }
        }

        if (GetActivePanel() == "Assessment3")
        {
          if (cloudDaysButton.Length > 0)
          {
            foreach (GameObject day in cloudDaysButton)
            {
              day.SetActive(false);
            }
          }
        }
      }
      else
      {
        playableDirector.Resume();
        if (GetActivePanel() == "Scene4") tracingObject.SetActive(true);

        if (GetActivePanel() == "Assessment2")
        {
          if (moleHole.Length > 0)
          {
            foreach (GameObject hole in moleHole)
            {
              hole.SetActive(true);
            }
          }
        }

        if (GetActivePanel() == "Assessment3")
        {
          if (cloudDaysButton.Length > 0)
          {
            foreach (GameObject day in cloudDaysButton)
            {
              day.SetActive(true);
            }
          }
        }
      }
    }
  }


  public void TogglePanel()
  {
    if (EventSystem.current.currentSelectedGameObject != null)
    {
      Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
      selectedButton.enabled = false;
    }

    for (int i = 0; i < Panels.Length; i++)
    {
      if (Panels[i].activeInHierarchy && (i + 1) < Panels.Length)
      {
        Panels[i].SetActive(false);
        Panels[i + 1].SetActive(true);

        if (Panels[i + 1].transform.name == "Scene4")
        pencil = GameObject.Find("Pencil");

        if (Panels[i + 1].transform.name == "Assessment2")
        {
          Quarter3Level3 gameManager = FindObjectOfType<Quarter3Level3>();

          if (gameManager != null)
          {
            gameManager.StartCoroutine(gameManager.InfiniteMoleSpawner());
          }
        }

        if (Panels[i + 1].transform.name == "Assessment1" || Panels[i + 1].transform.name == "Result")
        ToggleProgressBar();
        break;
      }
    }
  }


  private string GetActivePanel()
  {
    for (int i = 0; i < Panels.Length; i++)
    {
      if (Panels[i].activeInHierarchy)
      {
        PlayerPrefs.SetInt("CurrentPanel", i);
        return Panels[i].transform.name;
      }
    }
    return "None";
  }


  public GameObject ProgressBar;

  private void ToggleProgressBar()
  {
    ProgressBar.SetActive(!ProgressBar.activeInHierarchy);
  }


  static int starCount;
  public GameObject Result;
  public GameObject ZeroStar;
  public GameObject OneStar;
  public GameObject TwoStars;
  public GameObject ThreeStars;

  private void ToggleResult()
  {
    PlayerPrefs.SetFloat("Theme3 Score", totalProgressFill);

    if (PlayerPrefs.HasKey("Theme3 Score"))
    {
      Result.SetActive(true);

      if (starCount == 0)
      {
        PlayerPrefs.SetInt("Delay Time", 7);
        ZeroStar.SetActive(true);
      }
      else if (starCount == 1)
      {
        PlayerPrefs.SetInt("Delay Time", 12);
        OneStar.SetActive(true);
      }
      else if (starCount == 2)
      {
        PlayerPrefs.SetInt("Delay Time", 12);
        TwoStars.SetActive(true);
      }
      else if (starCount == 3)
      {
        PlayerPrefs.SetInt("Delay Time", 16);
        ThreeStars.SetActive(true);
      }
    }
    
  }
  // -------------------------------------------------- //

  // LESSON COMMANDS
  // -------------------------------------------------- //
  public GameObject Pencil;
  public GameObject ShortPadPaper;
  public GameObject GreenNotebook;
  public GameObject Ruler;
  public GameObject Crayon;

  private int slot;
  private Vector3 slot1 = new Vector3(-74, -296, 0);
  private Vector3 slot2 = new Vector3(62, -296, 0);
  private Vector3 slot3 = new Vector3(195, -296, 0);
  private Vector3 slot4 = new Vector3(329, -296, 0);
  private Vector3 slot5 = new Vector3(426, -296, 0);

  private Vector3 pencilInitialPosition = new Vector3(129f, 94f, 0f);
  private Vector3 pencilInitialScale = new Vector3(0.25f, 0.25f, 0.25f);
  private Vector3 pencilScale = new Vector3(0.35f, 0.35f, 0.35f);
  private Vector3 pencilRotation = new Vector3(0f, 0f, 39.002f);

  private Vector3 rulerInitialPosition = new Vector3(400f, -109f, 0f);
  private Vector3 rulerInitialScale = new Vector3(0.7f, 0.7f, 0.7f);
  private Vector3 rulerScale = new Vector3(0.8f, 0.8f, 0.8f);
  private Vector3 rulerRotation = new Vector3(0f, 0f, 86.438f);

  private Vector3 crayonInitialPosition = new Vector3(603f, -117f, 0f);
  private Vector3 crayonInitialScale = new Vector3(0.7f, 0.7f, 0.7f);
  private Vector3 crayonScale = new Vector3(0.6f, 0.6f, 0.6f);
  private Vector3 crayonRotation = new Vector3(0f, 0f, -183.283f);

  private Vector3 shortPadPaperInitialPosition = new Vector3(610f, 112f, 0f);
  private Vector3 shortPadPaperRotation = new Vector3(0f, 0f, 38.29f);

  private Vector3 greenNotebookInitialPosition = new Vector3(63f, -108f, 0f);

  public PlayableDirector Scene1;
  public TimelineAsset Scene1Part2Timeline;

  public void PickUpSchoolObject(string schoolObject)
  {
    slot++;
    PlaySFX("correct");
    StartCoroutine(GoToDesignatedPosition(schoolObject, slot));

    if (slot == 5)
    {
      Scene1.playableAsset = Scene1Part2Timeline;
      Scene1.Play();
    }
  }

  private IEnumerator GoToDesignatedPosition(string schoolObject, int designatedSlot)
  {
    if (schoolObject == "pencil")
    {
      Pencil.transform.SetAsLastSibling();
      RectTransform pencilRectTransform = Pencil.GetComponent<RectTransform>();

      float elapsedTime = 0f;
      float transitionDuration = 0.5f;

      while (elapsedTime < transitionDuration)
      {
        if (slot == 1)
        {
          pencilRectTransform.localPosition = Vector3.Lerp(
            pencilInitialPosition,
            slot1,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 2)
        {
          pencilRectTransform.localPosition = Vector3.Lerp(
            pencilInitialPosition,
            slot2,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 3)
        {
          pencilRectTransform.localPosition = Vector3.Lerp(
            pencilInitialPosition,
            slot3,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 4)
        {
          pencilRectTransform.localPosition = Vector3.Lerp(
            pencilInitialPosition,
            slot4,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 5)
        {
          pencilRectTransform.localPosition = Vector3.Lerp(
            pencilInitialPosition,
            slot5,
            elapsedTime / transitionDuration
          );
        }

        pencilRectTransform.localScale = Vector3.Lerp(
          pencilInitialScale,
          pencilScale,
          elapsedTime / transitionDuration
        );

        pencilRectTransform.eulerAngles = Vector3.Lerp(
          new Vector3(0f, 0f, 129.965f),
          pencilRotation,
          elapsedTime / transitionDuration
        );

        elapsedTime += Time.deltaTime;
        yield return null;
      }

      if (slot == 1) pencilRectTransform.localPosition = slot1;
      else if (slot == 2) pencilRectTransform.localPosition = slot2;
      else if (slot == 3) pencilRectTransform.localPosition = slot3;
      else if (slot == 4) pencilRectTransform.localPosition = slot4;
      else if (slot == 5) pencilRectTransform.localPosition = slot5;

      pencilRectTransform.localScale = pencilScale;
      pencilRectTransform.eulerAngles = pencilRotation;
    }
    else if (schoolObject == "ruler")
    {
      Ruler.transform.SetAsLastSibling();
      RectTransform rulerRectTransform = Ruler.GetComponent<RectTransform>();

      float elapsedTime = 0f;
      float transitionDuration = 0.5f;

      while (elapsedTime < transitionDuration)
      {
        if (slot == 1)
        {
          rulerRectTransform.localPosition = Vector3.Lerp(
            rulerInitialPosition,
            slot1,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 2)
        {
          rulerRectTransform.localPosition = Vector3.Lerp(
            rulerInitialPosition,
            slot2,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 3)
        {
          rulerRectTransform.localPosition = Vector3.Lerp(
            rulerInitialPosition,
            slot3,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 4)
        {
          rulerRectTransform.localPosition = Vector3.Lerp(
            rulerInitialPosition,
            slot4,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 5)
        {
          rulerRectTransform.localPosition = Vector3.Lerp(
            rulerInitialPosition,
            slot5,
            elapsedTime / transitionDuration
          );
        }

        rulerRectTransform.localScale = Vector3.Lerp(
          rulerInitialScale,
          rulerScale,
          elapsedTime / transitionDuration
        );

        rulerRectTransform.eulerAngles = Vector3.Lerp(
          new Vector3(0f, 0f, 0f),
          rulerRotation,
          elapsedTime / transitionDuration
        );

        elapsedTime += Time.deltaTime;
        yield return null;
      }

      if (slot == 1) rulerRectTransform.localPosition = slot1;
      else if (slot == 2) rulerRectTransform.localPosition = slot2;
      else if (slot == 3) rulerRectTransform.localPosition = slot3;
      else if (slot == 4) rulerRectTransform.localPosition = slot4;
      else if (slot == 5) rulerRectTransform.localPosition = slot5;

      rulerRectTransform.localScale = rulerScale;
      rulerRectTransform.eulerAngles = rulerRotation;
    }
    else if (schoolObject == "crayon")
    {
      Crayon.transform.SetAsLastSibling();
      RectTransform crayonRectTransform = Crayon.GetComponent<RectTransform>();

      float elapsedTime = 0f;
      float transitionDuration = 0.5f;

      while (elapsedTime < transitionDuration)
      {
        if (slot == 1)
        {
          crayonRectTransform.localPosition = Vector3.Lerp(
            crayonInitialPosition,
            slot1,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 2)
        {
          crayonRectTransform.localPosition = Vector3.Lerp(
            crayonInitialPosition,
            slot2,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 3)
        {
          crayonRectTransform.localPosition = Vector3.Lerp(
            crayonInitialPosition,
            slot3,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 4)
        {
          crayonRectTransform.localPosition = Vector3.Lerp(
            crayonInitialPosition,
            slot4,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 5)
        {
          crayonRectTransform.localPosition = Vector3.Lerp(
            crayonInitialPosition,
            slot5,
            elapsedTime / transitionDuration
          );
        }

        crayonRectTransform.localScale = Vector3.Lerp(
          crayonInitialScale,
          crayonScale,
          elapsedTime / transitionDuration
        );

        crayonRectTransform.eulerAngles = Vector3.Lerp(
          new Vector3(0f, 0f, 0f),
          crayonRotation,
          elapsedTime / transitionDuration
        );

        elapsedTime += Time.deltaTime;
        yield return null;
      }

      if (slot == 1) crayonRectTransform.localPosition = slot1;
      else if (slot == 2) crayonRectTransform.localPosition = slot2;
      else if (slot == 3) crayonRectTransform.localPosition = slot3;
      else if (slot == 4) crayonRectTransform.localPosition = slot4;
      else if (slot == 5) crayonRectTransform.localPosition = slot5;

      crayonRectTransform.localScale = crayonScale;
      crayonRectTransform.eulerAngles = crayonRotation;
    }
    else if (schoolObject == "short pad paper")
    {
      ShortPadPaper.transform.SetAsLastSibling();
      RectTransform shortPadPaperRectTransform = ShortPadPaper.GetComponent<RectTransform>();

      float elapsedTime = 0f;
      float transitionDuration = 0.5f;

      while (elapsedTime < transitionDuration)
      {
        if (slot == 1)
        {
          shortPadPaperRectTransform.localPosition = Vector3.Lerp(
            shortPadPaperInitialPosition,
            slot1,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 2)
        {
          shortPadPaperRectTransform.localPosition = Vector3.Lerp(
            shortPadPaperInitialPosition,
            slot2,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 3)
        {
          shortPadPaperRectTransform.localPosition = Vector3.Lerp(
            shortPadPaperInitialPosition,
            slot3,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 4)
        {
          shortPadPaperRectTransform.localPosition = Vector3.Lerp(
            shortPadPaperInitialPosition,
            slot4,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 5)
        {
          shortPadPaperRectTransform.localPosition = Vector3.Lerp(
            shortPadPaperInitialPosition,
            slot5,
            elapsedTime / transitionDuration
          );
        }

        shortPadPaperRectTransform.eulerAngles = Vector3.Lerp(
          new Vector3(0f, 0f, 0f),
          shortPadPaperRotation,
          elapsedTime / transitionDuration
        );

        elapsedTime += Time.deltaTime;
        yield return null;
      }

      if (slot == 1) shortPadPaperRectTransform.localPosition = slot1;
      else if (slot == 2) shortPadPaperRectTransform.localPosition = slot2;
      else if (slot == 3) shortPadPaperRectTransform.localPosition = slot3;
      else if (slot == 4) shortPadPaperRectTransform.localPosition = slot4;
      else if (slot == 5) shortPadPaperRectTransform.localPosition = slot5;

      shortPadPaperRectTransform.eulerAngles = shortPadPaperRotation;
    }
    else if (schoolObject == "green notebook")
    {
      GreenNotebook.transform.SetAsLastSibling();
      RectTransform greenNotebookRectTransform = GreenNotebook.GetComponent<RectTransform>();

      float elapsedTime = 0f;
      float transitionDuration = 0.5f;

      while (elapsedTime < transitionDuration)
      {
        if (slot == 1)
        {
          greenNotebookRectTransform.localPosition = Vector3.Lerp(
            greenNotebookInitialPosition,
            slot1,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 2)
        {
          greenNotebookRectTransform.localPosition = Vector3.Lerp(
            greenNotebookInitialPosition,
            slot2,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 3)
        {
          greenNotebookRectTransform.localPosition = Vector3.Lerp(
            greenNotebookInitialPosition,
            slot3,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 4)
        {
          greenNotebookRectTransform.localPosition = Vector3.Lerp(
            greenNotebookInitialPosition,
            slot4,
            elapsedTime / transitionDuration
          );
        }
        else if (slot == 5)
        {
          greenNotebookRectTransform.localPosition = Vector3.Lerp(
            greenNotebookInitialPosition,
            slot5,
            elapsedTime / transitionDuration
          );
        }

        elapsedTime += Time.deltaTime;
        yield return null;
      }

      if (slot == 1) greenNotebookRectTransform.localPosition = slot1;
      else if (slot == 2) greenNotebookRectTransform.localPosition = slot2;
      else if (slot == 3) greenNotebookRectTransform.localPosition = slot3;
      else if (slot == 4) greenNotebookRectTransform.localPosition = slot4;
      else if (slot == 5) greenNotebookRectTransform.localPosition = slot5;
    }
  }
  // -------------------------------------------------- //

  // GAMING COMMANDS
  // -------------------------------------------------- //
  public void OnPointerDown(PointerEventData eventData)
  {
    if (GetActivePanel() == "Assessment1")
    {
      isDragging = true;
      schoolMaterialInitialPosition = schoolMaterial.transform.position;
      offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - schoolMaterial.transform.position;
      siblingIndex = schoolMaterial.transform.GetSiblingIndex();
      schoolMaterial.transform.SetAsLastSibling();
      schoolMaterialImage.raycastTarget = false;

      PlaySFX("button");
    }
  }


  public GameObject backpack;

  public void OnPointerUp(PointerEventData eventData)
  {
    if (GetActivePanel() == "Assessment1")
    {
      if (Vector2.Distance(schoolMaterial.transform.position, backpack.transform.position) < 200)
      {
        isDropped = true;
        if (Array.Exists(new string[] { "pencil", "pad paper - short", "notebook - green", "ruler", "crayon" }, name => name == schoolMaterial.transform.name))
        {
          schoolMaterial.transform.SetSiblingIndex(schoolMaterial.transform.parent.childCount - 2);

          StartCoroutine(GetInsideTheBag(schoolMaterial));

          AddPoints();
          return;
        }
        else
        {
          SubPoints();
        }
      }

      isDragging = false;
      isDropped = false;
      schoolMaterial.transform.position = schoolMaterialInitialPosition;
      schoolMaterial.transform.SetSiblingIndex(siblingIndex);
      schoolMaterialImage.raycastTarget = true;
    }
  }

  IEnumerator GetInsideTheBag(GameObject material)
  {
    float elapsedTime = 0f;
    float transitionDuration = 0.5f;

    while (elapsedTime < transitionDuration)
    {
      schoolMaterial.GetComponent<RectTransform>().localPosition = Vector3.Lerp(
        new Vector3(-225, 120, 0),
        new Vector3(-225, -133, 0),
        elapsedTime / transitionDuration
      );

      elapsedTime += Time.deltaTime;
      yield return null;
    }
  }


  public GameObject[] whack_a_number;
  public Sprite[] number_sprite;
  private Vector2 showPosition = Vector2.zero;
  private Vector2 hidePosition = new Vector2(0f, -170f);
  private System.Random random = new System.Random();

  private IEnumerator InfiniteMoleSpawner()
  {
    int[] biasedSpriteIndices = new int[] { 0, 1, 2, 3, 4, 5, 6, 6, 6, 7, 7, 7, 8, 9 };

    while (true)
    {
      try
      {
        int randomMoleIndex = random.Next(0, whack_a_number.Length);
        int randomSpriteIndex = biasedSpriteIndices[random.Next(0, biasedSpriteIndices.Length)];

        GameObject peekingMole = whack_a_number[randomMoleIndex];

        if (!peekingMole.activeInHierarchy)
        {
          whack_a_number[randomMoleIndex].GetComponent<SpriteRenderer>().sprite = number_sprite[randomSpriteIndex];
          StartCoroutine(SpawnAMole(peekingMole, showPosition, hidePosition));
        }
      }
      catch (IndexOutOfRangeException)
      {
        // do nothing
      }

      yield return new WaitForSeconds(random.Next(1, 3));
    }
  }


  private float spawnTransitionDuration = 0.5f;
  private float spawnDuration = 1f;

  private IEnumerator SpawnAMole(GameObject mole, Vector2 show, Vector2 hide)
  {
    mole.transform.localPosition = hide;
    mole.SetActive(true);

    float elapsedTime = 0f;
    while (elapsedTime < spawnTransitionDuration)
    {
      mole.transform.localPosition = Vector2.Lerp(hide, show, elapsedTime/spawnTransitionDuration);

      if (elapsedTime > 0.3f)
      {
        mole.GetComponent<CircleCollider2D>().enabled = true;
      }

      elapsedTime += Time.deltaTime;
      yield return null;
    }

    mole.transform.localPosition = show;

    yield return new WaitForSeconds(spawnDuration);

    elapsedTime = 0f;
    while (elapsedTime < spawnTransitionDuration)
    {
      mole.transform.localPosition = Vector2.Lerp(show, hide, elapsedTime / spawnTransitionDuration);

      if (elapsedTime > 0.2f)
      {
        mole.GetComponent<CircleCollider2D>().enabled = false;
      }

      elapsedTime += Time.deltaTime;
      yield return null;
    }

    mole.transform.localPosition = hide;
    mole.SetActive(false);
  }


  public GameObject sixCountText;
  public GameObject sevenCountText;
  private float sixCount;
  private float sevenCount;
  private float mistakeCount;

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.CompareTag("whack-a-number"))
    {
      SpriteRenderer spriteRenderer = collider.GetComponent<SpriteRenderer>();
      string spriteName = spriteRenderer.sprite.name;

      if (spriteName == "Whack-a-six")
      {
        if (sixCount < 6f)
        {
          sixCount++;
          sixCountText.GetComponent<TMP_Text>().text = sixCount.ToString();
          AddPoints();
        }
      }
      else if (spriteName == "Whack-a-seven")
      {
        if (sevenCount < 7f)
        {
          sevenCount++;
          sevenCountText.GetComponent<TMP_Text>().text = sevenCount.ToString();
          AddPoints();
        }
      }
      else if (mistakeCount < 13f)
      {
        mistakeCount++;
        SubPoints();
      }
    }
  }


  string[] cloudDays = new string[] { "linggo", "lunes", "martes", "miyerkules", "huwebes", "biyernes", "sabado" };
  List<string> clouds = new List<string>();
  private int dayNumber = 0;
  public GameObject[] cloudDaysButton;
  public GameObject[] cloudDaysImage;

  public void Cloudify(string cloud)
  {
    if (cloudDays[dayNumber] == cloud && !clouds.Contains(cloud))
    {
      AddPoints();
      clouds.Add(cloud);
      cloudDaysButton[dayNumber].SetActive(false);
      cloudDaysImage[dayNumber].SetActive(true);
      dayNumber++;
    }
    else
    {
      SubPoints();
    }
  }
  // -------------------------------------------------- //

  // SCORING
  // -------------------------------------------------- //
  static float starFillMultiplier = .33333f;

  static float assessment1 = 100f;
  static float a1_ProgressFill;
  static float a1_Points;
  static float a1_CorrectAnswers = 5f;
  static float a1_WrongAnswers = 8f;
  static float a1_PlayerScore;
  static float a1_PlayerMistakes;

  static float assessment2 = 100f;
  static float a2_ProgressFill;
  static float a2_Points;
  static float a2_CorrectAnswers = 13f;
  static float a2_PlayerScore;

  static float assessment3 = 100f;
  static float a3_ProgressFill;
  static float a3_Points;
  static float a3_CorrectAnswers = 7f;
  static float a3_PlayerScore;
  static float a3_PlayerMistakes;

  private void AddPoints()
  {
    if (GetActivePanel() == "Assessment1")
    {
      a1_Points += assessment1 / a1_CorrectAnswers;
      a1_PlayerScore++;
      Debug.Log("A1: " + a1_Points);

      a1_ProgressFill = (a1_Points / 100f) * starFillMultiplier;
      if (a1_PlayerScore == a1_CorrectAnswers) TogglePanel();

      PlaySFX("correct");
      OnProgress();
    }
    else if (GetActivePanel() == "Assessment2")
    {
      a2_Points += assessment2 / a2_CorrectAnswers;
      a2_PlayerScore++;
      if (a2_Points == 99.99999f) a2_Points += 0.00001f; // ensures 100%

      Debug.Log("A2: " + a2_Points);

      a2_ProgressFill = (a2_Points / 100f) * starFillMultiplier;
      if (a2_PlayerScore == a2_CorrectAnswers) TogglePanel();

      PlaySFX("correct");
      OnProgress();
    }
    else if (GetActivePanel() == "Assessment3")
    {
      a3_Points += assessment3 / a3_CorrectAnswers;
      a3_PlayerScore++;
      if (a3_Points == 99.99999f) a3_Points += 0.00001f; // ensures 100%
  
      Debug.Log("A3: " + a3_Points);

      a3_ProgressFill = (a3_Points / 100f) * starFillMultiplier;
      if (a3_ProgressFill == 0.33333f) a3_ProgressFill += 0.00001f; // ensures 1f for totalProgressFill

      PlaySFX("correct");
      OnProgress();

      if (a3_PlayerScore == a3_CorrectAnswers)
      {
        GameObject.Find("Assessment3").SetActive(false);
        ToggleResult();
      }
    }
  }


  private void SubPoints()
  {
    if (GetActivePanel() == "Assessment1")
    {
      if (a1_PlayerMistakes != a1_WrongAnswers)
      {
        a1_Points -= assessment1 / a1_WrongAnswers;
        Debug.Log("A1: " + a1_Points);
        a1_PlayerMistakes++;
        a1_ProgressFill = (a1_Points / 100f) * starFillMultiplier;

        PlaySFX("wrong");
        OnProgress();
      };
    }
    else if (GetActivePanel() == "Assessment2")
    {
      a2_Points -= assessment2 / a2_CorrectAnswers;
      Debug.Log("A2: " + a2_Points);

      a2_ProgressFill = (a2_Points / 100f) * starFillMultiplier;

      PlaySFX("wrong");
      OnProgress();
    }
    else if (GetActivePanel() == "Assessment3")
    {
      if (a3_PlayerMistakes != a3_CorrectAnswers) {
        a3_Points -= assessment3 / a3_CorrectAnswers;
        Debug.Log("A3: " + a3_Points);
        a3_PlayerMistakes++;
        a3_ProgressFill = (a3_Points / 100f) * starFillMultiplier;

        PlaySFX("wrong");
        OnProgress();
      }
    }
  }

  public Image ProgressBarMask;
  public Sprite EarnedStar;
  public Sprite UnearnedStar;
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
    else if (totalProgressFill >= .66666f && totalProgressFill < 1f)
    {
      UnearnedStarImages[0].sprite = EarnedStar;
      UnearnedStarImages[1].sprite = EarnedStar;
      UnearnedStarImages[2].sprite = UnearnedStar;
      starCount = 2;
      Debug.Log("Stars: " + starCount);
    }
    else if (totalProgressFill >= .33333f && totalProgressFill < .66666f)
    {
      UnearnedStarImages[0].sprite = EarnedStar;
      UnearnedStarImages[1].sprite = UnearnedStar;
      UnearnedStarImages[2].sprite = UnearnedStar;

      starCount = 1;
      Debug.Log("Stars: " + starCount);
    }
    else if (totalProgressFill < .33333f)
    {
      UnearnedStarImages[0].sprite = UnearnedStar;
      UnearnedStarImages[1].sprite = UnearnedStar;
      UnearnedStarImages[2].sprite = UnearnedStar;
    }
    //PlayerPrefs.SetFloat("Theme3 Score", totalProgressFill);
  }
  // -------------------------------------------------- //

  // PLAYING BGM AND SFX
  // -------------------------------------------------- //
  [SerializeField] public AudioSource audioSource;
  [SerializeField] public AudioClip button, correct, wrong, cartoonHammer;

  public void PlaySFX(string sfx)
  {
    if (sfx == "button") audioSource.PlayOneShot(button);
    else if (sfx == "correct") audioSource.PlayOneShot(correct);
    else if (sfx == "wrong") audioSource.PlayOneShot(wrong);
    else if (sfx == "cartoon hammer") {
      audioSource.clip = cartoonHammer;
      audioSource.Stop();
      audioSource.Play();
    }
  }
  // -------------------------------------------------- //
}
