using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SelectAccount : MonoBehaviour
{
    public void onButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
