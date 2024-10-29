using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Star_display : MonoBehaviour
{
    public GameObject star1_display;
    public GameObject star2_display;
    public GameObject star3_display;
    public TextMeshProUGUI complimentary_textBox;
    private Button_Manager buttonManager;

    void Start()
    {        
        buttonManager = FindObjectOfType<Button_Manager>();
        Debug.Log("Counter value: " + buttonManager.score);
        UpdateStarVisibility();
    }

    void UpdateStarVisibility()
    {

        switch (buttonManager.score)
        {
            case 1:
                star1_display.SetActive(true);
                star2_display.SetActive(false);
                star3_display.SetActive(false);
                complimentary_textBox.text = "SUBOK";
                break;
            case 2:
                star1_display.SetActive(false);
                star2_display.SetActive(true);
                star3_display.SetActive(false);
                complimentary_textBox.text = "MAGALING";
                break;
            case 3:
                star1_display.SetActive(false);
                star2_display.SetActive(false);
                star3_display.SetActive(true);
                complimentary_textBox.text = "PERPEKTO";
                break;
            default:
                star1_display.SetActive(false);
                star2_display.SetActive(false);
                star3_display.SetActive(false);
                complimentary_textBox.text = "WAWA";
                break;
        }
        //}
        //else
        //{
        //    switch (buttonManager.score)
        //    {
        //        case 3:
        //            star1_display.SetActive(true);
        //            star2_display.SetActive(false);
        //            star3_display.SetActive(false);
        //            complimentary_textBox.text = "SUBOK";
        //            break;
        //        case 4:
        //            star1_display.SetActive(false);
        //            star2_display.SetActive(true);
        //            star3_display.SetActive(false);
        //            complimentary_textBox.text = "MAGALING";
        //            break;
        //        case 5:
        //            star1_display.SetActive(false);
        //            star2_display.SetActive(false);
        //            star3_display.SetActive(true);
        //            complimentary_textBox.text = "PERPEKTO";
        //            break;
        //        default:
        //            star1_display.SetActive(false);
        //            star2_display.SetActive(false);
        //            star3_display.SetActive(false);
        //            complimentary_textBox.text = "WAWA";
        //            break;
        //    }
        //}
    }
}
