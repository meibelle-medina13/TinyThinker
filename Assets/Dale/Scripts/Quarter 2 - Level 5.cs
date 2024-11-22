using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quarter2Level5 : MonoBehaviour
{
  // SWITCHING PANELS
  // -------------------------------------------------- //
  public GameObject[] Panels;

  void Start()
  {

  }


  public GameObject Pencil;
  public GameObject PencilMask;
  public GameObject Collider;
  private Vector3 pencilState;
  private Vector3 pencilRaise = new Vector3(105, 120, 0);
  private Vector3 pencilWrite = new Vector3(85, 100, 0);

  void Update()
  {
    if (GetActivePanel() == "Assessment2" || GetActivePanel() == "Assessment3")
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


  public void TogglePanel()
  {
    for (int i = 0; i < Panels.Length; i++)
    {
      if (Panels[i].activeInHierarchy && (i + 1) < Panels.Length)
      {
        Panels[i].SetActive(false);
        Panels[i + 1].SetActive(true);

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
      if (Panels[i].activeInHierarchy) return Panels[i].transform.name;
    }
    return "None";
  }


  public GameObject ProgressBar;

  private void ToggleProgressBar()
  {
    ProgressBar.SetActive(!ProgressBar.activeInHierarchy);
  }
  // -------------------------------------------------- //


  // GAMING COMMANDS
  // -------------------------------------------------- //
  HashSet<string> tracedPoints = new HashSet<string>();

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.CompareTag("Tracing Point") && !tracedPoints.Contains(collider.gameObject.name))
    {
      tracedPoints.Add(collider.gameObject.name);
    }
  }
  // -------------------------------------------------- //

  // SCORING
  // -------------------------------------------------- //

  // -------------------------------------------------- //

  // PLAYING BGM AND SFX
  // -------------------------------------------------- //

  // -------------------------------------------------- //
}
