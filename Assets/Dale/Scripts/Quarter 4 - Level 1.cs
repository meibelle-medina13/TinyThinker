using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Quarter4Level1 : MonoBehaviour
{
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
