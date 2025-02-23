using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverGameSettings : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject[] animations = new GameObject[2];

    public void OnPointerEnter(PointerEventData eventData)
    {
        animations[0].SetActive(true);
        animations[1].SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animations[1].SetActive(true);
        animations[0].SetActive(false);
    }
}
