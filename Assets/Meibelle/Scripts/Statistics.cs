using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    [Header("<---- PANELS and GAMEOBJECTS ---->")]
    [SerializeField] private GameObject levelStatisticsPanel;
    [SerializeField] private GameObject themeLabel;
    [SerializeField] private TextMeshProUGUI themeText;
    [SerializeField] private TextMeshProUGUI[] percentScore = new TextMeshProUGUI[5];
    [SerializeField] private Image[] statsBar = new Image[5];
    [SerializeField] private GameObject[] level1Star = new GameObject[3];
    [SerializeField] private GameObject[] level2Star = new GameObject[3];
    [SerializeField] private GameObject[] level3Star = new GameObject[3];
    [SerializeField] private GameObject[] level4Star = new GameObject[3];
    [SerializeField] private GameObject[] level5Star = new GameObject[3];

    [Header("<---- SPRITES ---->")]
    [SerializeField] private Sprite[] bgImages = new Sprite[4];
    [SerializeField] private Sprite[] labelSprites = new Sprite[4];

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField] private STATISTICS_RESPONSES requestsManager;

    int userID;

    private void Start()
    {
        requestsManager = FindObjectOfType<STATISTICS_RESPONSES>();
        userID = PlayerPrefs.GetInt("Current_user");
    }
    public void OpenLevelStatistic(int theme_num)
    {
        levelStatisticsPanel.SetActive(true);

        levelStatisticsPanel.GetComponent<Image>().sprite = bgImages[theme_num-1];
        themeLabel.GetComponent<Image>().sprite = labelSprites[theme_num-1];
        themeText.text = "TEMA " + (theme_num).ToString();
        StartCoroutine(GetStatistics(theme_num));
    }

    IEnumerator GetStatistics(int theme_num)
    {
        yield return StartCoroutine(requestsManager.GetLevelScores("/statistic", userID, theme_num));

        if (requestsManager.json != null)
        {
            for (int j = 0; j < 5; j++)
            {
                percentScore[j].text = "0%";
                statsBar[j].fillAmount = 0;
                foreach (GameObject star in level1Star)
                {
                    star.SetActive(false);
                }
                foreach (GameObject star in level2Star)
                {
                    star.SetActive(false);
                }
                foreach (GameObject star in level3Star)
                {
                    star.SetActive(false);
                }
                foreach (GameObject star in level4Star)
                {
                    star.SetActive(false);
                }
                foreach (GameObject star in level5Star)
                {
                    star.SetActive(false);
                }
            }
            for (int i = 0; i < requestsManager.json.data.Count; i++)
            {
                percentScore[i].text = requestsManager.json.data[i].scores.ToString() +"%";
                statsBar[i].fillAmount = (float) requestsManager.json.data[i].scores / 100f;
                if (i+1 == 1)
                {
                    if (requestsManager.json.data[i].scores >= 33 &&
                        requestsManager.json.data[i].scores < 66.666666f)
                    {
                        level1Star[0].SetActive(true);
                    }
                    else if (requestsManager.json.data[i].scores >= 66 &&
                        requestsManager.json.data[i].scores < 99.99999f)
                    {
                        level1Star[1].SetActive(true);
                    }
                    else if (requestsManager.json.data[i].scores == 100)
                    {
                        level1Star[2].SetActive(true);
                    }
                }
                else if (i+1 == 2)
                {
                    if (requestsManager.json.data[i].scores >= 33 &&
                        requestsManager.json.data[i].scores < 66.666666f)
                    {
                        level2Star[0].SetActive(true);
                    }
                    else if (requestsManager.json.data[i].scores >= 66 &&
                        requestsManager.json.data[i].scores < 99.99999f)
                    {
                        level2Star[1].SetActive(true);
                    }
                    else if (requestsManager.json.data[i].scores == 100)
                    {
                        level2Star[2].SetActive(true);
                    }
                }
                else if (i+1 == 3)
                {
                    if (requestsManager.json.data[i].scores >= 33 &&
                        requestsManager.json.data[i].scores < 66.666666f)
                    {
                        level3Star[0].SetActive(true);
                    }
                    else if (requestsManager.json.data[i].scores >= 66 &&
                        requestsManager.json.data[i].scores < 99.99999f)
                    {
                        level3Star[1].SetActive(true);
                    }
                    else if (requestsManager.json.data[i].scores == 100)
                    {
                        level3Star[2].SetActive(true);
                    }
                }
                else if (i + 1 == 4)
                {
                    if (requestsManager.json.data[i].scores >= 33 &&
                        requestsManager.json.data[i].scores < 66.666666f)
                    {
                        level4Star[0].SetActive(true);
                    }
                    else if (requestsManager.json.data[i].scores >= 66 &&
                        requestsManager.json.data[i].scores < 99.99999f)
                    {
                        level4Star[1].SetActive(true);
                    }
                    else if (requestsManager.json.data[i].scores == 100)
                    {
                        level4Star[2].SetActive(true);
                    }
                }
                else if (i + 1 == 5)
                {
                    if (requestsManager.json.data[i].scores >= 33 &&
                        requestsManager.json.data[i].scores < 66.666666f)
                    {
                        level5Star[0].SetActive(true);
                    }
                    else if (requestsManager.json.data[i].scores >= 66 &&
                        requestsManager.json.data[i].scores < 99.99999f)
                    {
                        level5Star[1].SetActive(true);
                    }
                    else if (requestsManager.json.data[i].scores == 100)
                    {
                        level5Star[2].SetActive(true);
                    }
                }
            }
        }
    }
    public void GoToThemeButtons()
    {
        levelStatisticsPanel.SetActive(false);
    }

    public void GoBackMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(7);
    }
}
