using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ThemeMap : MonoBehaviour
{
    [Header("<---- LOADING PANEL ---->")]
    [SerializeField] private GameObject loadingScene;

    [Header("<---- TREASURE AND STICKERS ---->")]
    [SerializeField] private Sprite openTreasure;
    [SerializeField] private Sprite closeTreasure;
    [SerializeField] private GameObject shadowStickerPlacement;
    [SerializeField] private GameObject positionedStickerPlacement;
    [SerializeField] private GameObject stickerContainer;
    [SerializeField] private GameObject[] availableStickers = new GameObject[8];
    [SerializeField] private GameObject[] shadowStickers = new GameObject[8];
    [SerializeField] private GameObject[] positionedStickers = new GameObject[8];
    [SerializeField] private GameObject[] usedStickers = new GameObject[8];
    Vector3[] stickerInitialPos = new Vector3[8];

    [Header("<---- CURRENT THEME LOCATIONS ---->")]
    [SerializeField] private GameObject[] locations = new GameObject[4];

    [Header("<---- THEME BUTTONS AND LOCK ---->")]
    [SerializeField] private GameObject[] buttons = new GameObject[4];
    [SerializeField] private Sprite lockButton;
    [SerializeField] private Sprite selectButton;

    [Header("<---- THEME BUTTONS AND LOCK ---->")]
    [SerializeField] private GameObject settings;

    [Header("<---- GUIDE GAMEOBJECTS ---->")]
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject guideGameObject;

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField] private THEME_REQUEST requestsManager;

    int current_theme;
    int userID;
    DateTime currentDate;

    void Start()
    {
        userID = PlayerPrefs.GetInt("Current_user");
        currentDate = DateTime.Now;
        string dateString = currentDate.ToString("yyyy-MM-dd");

        if (PlayerPrefs.GetString("Current Date") != dateString)
        {
            PlayerPrefs.SetFloat(userID.ToString() + "Time", 7200);
            PlayerPrefs.SetString("Current Date", dateString);
            Debug.Log("Current Date"+ dateString);
        }

        current_theme = PlayerPrefs.GetInt("Current_theme");
        if (current_theme < 5)
        {
            locations[current_theme - 1].SetActive(true);
        }
        else
        {
            locations[3].SetActive(true);
        }

        if (PlayerPrefs.HasKey("StartGuide"+userID.ToString()) && PlayerPrefs.GetString("StartGuide" + userID.ToString()) == "True")
        {
            guideGameObject.SetActive(true);
            background.GetComponent<PlayableDirector>().enabled = true;
            locations[current_theme - 1].SetActive(false);
        }

        requestsManager = FindObjectOfType<THEME_REQUEST>();
        loadingScene.SetActive(false);

        for (int i = 0; i < availableStickers.Length; i++)
        {
            stickerInitialPos[i] = availableStickers[i].transform.position;
        }

        for (int i = 0; i < availableStickers.Length; ++i)
        {
            if (PlayerPrefs.GetString(userID.ToString() + "-" + (i+1).ToString()) == "True")
            {
                availableStickers[i].SetActive(true);
            }
        }

        for (int i = 0; i < positionedStickers.Length; ++i)
        {
            if(PlayerPrefs.GetString("Positioned" + userID.ToString() + positionedStickers[i].name) == "True")
            {
                positionedStickers[i].SetActive(true);
                availableStickers[i].SetActive(false);
                shadowStickers[i].SetActive(false);
                usedStickers[i].SetActive(true);
            }
        }
        StartCoroutine(CheckRewards());
        StartCoroutine(CheckQuarterAvailability());
    }

    private void Update()
    {
        if (PlayerPrefs.GetString("Showing") == "true" || PlayerPrefs.GetString("Paused") == "True")
        {
            if (current_theme < 5)
            {
                locations[current_theme - 1].SetActive(false);
            }
            else
            {
                locations[3].SetActive(false);
            }
        }
        else
        {
            if (current_theme < 5)
            {
                locations[current_theme - 1].SetActive(true);
            }
            else
            {
                locations[3].SetActive(true);
            }
        }

        if (PlayerPrefs.HasKey("StartGuide" + userID.ToString()) && PlayerPrefs.GetString("StartGuide" + userID.ToString()) == "True")
        {
            guideGameObject.SetActive(true);
            background.GetComponent<PlayableDirector>().enabled = true;
            PlayableDirector playableDirector = background.GetComponent<PlayableDirector>();
            if (playableDirector.state == PlayState.Paused)
            {
                guideGameObject.SetActive(false);
                background.GetComponent<PlayableDirector>().enabled = false;
                locations[current_theme - 1].SetActive(true);
                PlayerPrefs.SetString("StartGuide" + userID.ToString(), "False");
            }
        }
    }

    IEnumerator CheckRewards()
    {
        yield return StartCoroutine(requestsManager.GetRewards("/reward", userID));

        if (requestsManager.jsonReward != null)
        {
            for (int i = 0; i < requestsManager.jsonReward.data.Count; i++)
            {
                int reward_type_ID = requestsManager.jsonReward.data[i].reward_type_ID;
                PlayerPrefs.SetString(userID.ToString() + "-" + reward_type_ID.ToString(), "True");
                availableStickers[reward_type_ID-1].SetActive(true);
            }
        }
    }

    IEnumerator CheckQuarterAvailability()
    {
        yield return StartCoroutine(requestsManager.GetQuarterStatus("/quarter_status"));

        if (requestsManager.json != null)
        {
            for (int i = 0; i < requestsManager.json.data.Count; i++)
            {
                if (requestsManager.json.data[i].status == 0)
                {
                    buttons[i].GetComponent<Image>().sprite = lockButton;
                    buttons[i].GetComponentInChildren<Button>().interactable = false;
                }
                else
                {
                    if (current_theme >= i+1)
                    {
                        buttons[i].GetComponent<Image>().sprite = selectButton;
                        buttons[i].GetComponentInChildren<Button>().interactable = true;
                    }
                    else
                    {
                        buttons[i].GetComponent<Image>().sprite = lockButton;
                        buttons[i].GetComponentInChildren<Button>().interactable = false;
                    }
                }
            }
        }
    }

    public void SelectTheme(int theme_num)
    {
        PlayerPrefs.SetInt("Selected_theme", theme_num);
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        AsyncOperation asyncOperation;
        asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(7);

        while (!asyncOperation.isDone)
        {
            loadingScene.SetActive(true);
            if (current_theme < 5)
            {
                locations[current_theme - 1].SetActive(false);
            }
            else
            {
                locations[3].SetActive(false);
            }
            yield return null;
        }
    }

    public void ClickTreasure(GameObject treasure)
    {
        Debug.Log(treasure.GetComponent<Image>().sprite.name);
        if (treasure.GetComponent<Image>().sprite.name == "treasure-0")
        {
            treasure.GetComponent<Image>().sprite = openTreasure;
            shadowStickerPlacement.SetActive(true);
            stickerContainer.SetActive(true);
            settings.SetActive(false);
        }
        else
        {
            treasure.GetComponent<Image>().sprite = closeTreasure;
            shadowStickerPlacement.SetActive(false);
            stickerContainer.SetActive(false);
            settings.SetActive(true);
        }
    }

    public void DragSticker(GameObject sticker)
    {
        sticker.transform.position = Input.mousePosition;
    }

    public void DropSticker(GameObject sticker)
    {
        for (int i = 0; i < shadowStickers.Length; i++)
        {
            string[] shadowStickerName = shadowStickers[i].name.Split('-');
            string stickerName = shadowStickerName[0];
            if (sticker.name == stickerName)
            {
                float Distance = Vector3.Distance(sticker.transform.position, shadowStickers[i].transform.position);
                Debug.Log(Distance);
                if (Distance <= 50)
                {
                    sticker.SetActive(false);
                    shadowStickers[i].SetActive(false);
                    positionedStickers[i].SetActive(true);
                    usedStickers[i].SetActive(true);
                    PlayerPrefs.SetString("Positioned" + userID.ToString() + stickerName, "True");
                }
                else
                {
                    sticker.transform.position = stickerInitialPos[i];
                }
                break;
            }
        }
    }

    public void LogOut()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }
}
