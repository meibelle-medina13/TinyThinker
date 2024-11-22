using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreScript : MonoBehaviour
{
    int userID, Test_Score;

    public void GetTotalScore()
    {
        Test_Score = PlayerPrefs.GetInt("Test Score");
        Debug.Log("FInal:" + Test_Score);
        userID = PlayerPrefs.GetInt("Current_user");
        StartCoroutine(UpdateCurrentLevel());

    }

    // -------------------------------------------------------------------- //

    //int delaytime;
    //public IEnumerator GoToMap()
    //{
    //    yield return new WaitForSeconds(2);
    //    Debug.Log("Go to map");
    //    StartCoroutine(UpdateCurrentLevel());
    //}

    IEnumerator UpdateCurrentLevel()
    {
        int current_level = 1;
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes("{\"userID\": " + userID + ", \"current_level\": " + current_level + "}");

        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users", rawData))
        {
            www.method = "PUT";
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                PlayerPrefs.SetInt("Current_level", current_level);
                Debug.Log("Received: " + www.downloadHandler.text);
                UnityEngine.SceneManagement.SceneManager.LoadScene(7);
            }
        }
    }
}
