using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Quarter4Level2 : MonoBehaviour
{
  // Puro ToggleTimeline sa Scene4. May naghuhukay, may nagtatabon ng hukay, ta's may nagdidilig.
  // Part2, Part3, at Part4 'yon. Bale ibalik mo na lang sa dating lalagyan yung shovel at watering
  // can. Medyo hindi ko matantya transition kada timeline eh. Wala sa dating ayos kapag nakaplay
  // mismo pero maayos kapag sa timeline lang pinlay

  // Sa Scene8, ToggleTimeline Part2 pagkatapos magtrace para kita pa rin yung tinrace nung player

  // Sa Assessment1, text ginamit ko sa counter ng apple para madaling palitan yung value.
  // yung basket, nilagyan ko na ng apple para hindi ka na mahirapan magposition. Ikaw na bahala
  // kung magagamit 'yon o hindi

  // Goodluck sa Assessment2

  //
  // 'Wag na mataranta, tapos na lahat ng animation niyan :>
  //

  public GameObject[] Panels;

  public void ToggleNextPanel()
  {
    for (int i = 0; i < Panels.Length; i++)
    {
      if (Panels[i].activeInHierarchy && (i + 1) < Panels.Length)
      {
        Panels[i].SetActive(false);
        Panels[i + 1].SetActive(true);
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
