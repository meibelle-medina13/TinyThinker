using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject loaderUI;
    public Slider progressSlider;
    public float loadingSpeedMultiplier = 0.2f; 

    public void LoadScene(int index)
    {
        StartCoroutine(LoadScene_Coroutine(index));
    }

    public IEnumerator LoadScene_Coroutine(int index)
    {
        AsyncOperation asyncOperation;
        progressSlider.value = 0;
        loaderUI.SetActive(true);

        if (PlayerPrefs.HasKey("Email"))
        {
            asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(5);
        }
        else
        {
            asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
        }
        
        asyncOperation.allowSceneActivation = false;
        float progress = 0;

        while (!asyncOperation.isDone)
        {
         
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime * loadingSpeedMultiplier);
            progressSlider.value = progress;

            if (progress >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
