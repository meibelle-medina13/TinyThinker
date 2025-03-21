using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Start()
    {
        loadingScene.SetActive(false);

        for (int i = 0; i < availableStickers.Length; i++)
        {
            stickerInitialPos[i] = availableStickers[i].transform.position;
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
        }
        else
        {
            treasure.GetComponent<Image>().sprite = closeTreasure;
            shadowStickerPlacement.SetActive(false);
            stickerContainer.SetActive(false);
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
                }
                else
                {
                    sticker.transform.position = stickerInitialPos[i];
                }
                break;
            }
        }
    }
}
