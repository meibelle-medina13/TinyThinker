using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreScript : MonoBehaviour
{
    int userID, Test_Score;
    public int theme, testType;

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private PREPOST_TEST_REQUESTS requestsManager;

    private void Start()
    {
        requestsManager = FindObjectOfType<PREPOST_TEST_REQUESTS>();
    }

    public void GetTotalScore()
    {
        Test_Score = PlayerPrefs.GetInt("Test Score");
        Debug.Log("FInal:" + Test_Score);
        userID = PlayerPrefs.GetInt("Current_user");
        //StartCoroutine(requestsManager.updateTestScore("/test_score", userID, theme, testType, Test_Score));

        if (testType == 1)
        {
            StartCoroutine(requestsManager.updateTestScore("/test_score", userID, theme, testType, Test_Score));
            StartCoroutine(requestsManager.UpdateCurrentLevel("/users/updateLevel", 1, userID));
        }
        else if (testType == 2)
        {
            StartCoroutine(requestsManager.updateTestScore("/test_score", userID, theme-1, testType, Test_Score));
            StartCoroutine(requestsManager.UpdateCurrentLevel("/users/updateLevel", 0, userID));
            StartCoroutine(requestsManager.UpdateCurrentTheme("/users/updateTheme", userID, theme));
        }
    }

    // -------------------------------------------------------------------- //
}
