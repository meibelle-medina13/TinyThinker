using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Theme_Selection : MonoBehaviour
{
    [SerializeField]
    private Button select_button;

    public void Select_Theme(int theme_num)
    {
        if (theme_num == 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(6);
        }
    }
}
