using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverGameSettings : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject[] animations = new GameObject[2];
    [SerializeField]
    private GameObject[] themes = new GameObject[4];
    [SerializeField]
    private GameObject backgroundPanel;

    string[] signages = { "Q1_Signage", "Q2_Signage", "Q3_Signage", "Q4_Signage" };
    string[] themeCards = { "THEME1", "THEME2", "THEME3", "THEME4" };

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.name == "settings")
        {
            animations[0].SetActive(true);
            animations[1].SetActive(false);
        }
        else if (gameObject.name == "Q1_Signage" || gameObject.name == "THEME1")
        {
            PlayerPrefs.SetString("Showing", "true");
            themes[0].SetActive(true);
        }
        else if (gameObject.name == "Q2_Signage" || gameObject.name == "THEME2")
        {
            PlayerPrefs.SetString("Showing", "true");
            themes[1].SetActive(true);
        }
        else if (gameObject.name == "Q3_Signage" || gameObject.name == "THEME3")
        {
            PlayerPrefs.SetString("Showing", "true");
            themes[2].SetActive(true);
        }
        else if (gameObject.name == "Q4_Signage" || gameObject.name == "THEME4")
        {
            PlayerPrefs.SetString("Showing", "true");
            themes[3].SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.name == "settings")
        {
            animations[1].SetActive(true);
            animations[0].SetActive(false);
        }
        else if (signages.Contains(gameObject.name) || themeCards.Contains(gameObject.name))
        {
            PlayerPrefs.SetString("Showing", "false");
        }
    }

    private void Update()
    {
        if (gameObject.name != "settings")
        {
            if(PlayerPrefs.HasKey("Showing"))
            {
                backgroundPanel.SetActive(true);
                if (PlayerPrefs.GetString("Showing") == "false")
                {
                    backgroundPanel.SetActive(false);
                    for (int i = 0; i < themes.Length; i++)
                    {
                        if (themes[i].activeInHierarchy)
                        {
                            themes[i].SetActive(false);
                        }
                    }
                }
            }
            
            //if (themeCards.Contains(gameObject.name) && gameObject.activeSelf)
            //{
            //    backgroundPanel.SetActive(true);
            //}
            ////else
            ////{
            ////    backgroundPanel.SetActive(false);
            ////}
        }
    }
}
