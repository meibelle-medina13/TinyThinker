using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using static SelectAccount;
using Unity.VisualScripting.Antlr3.Runtime;
using System;
using static Login;

public class Theme1_Map : MonoBehaviour
{
    [SerializeField]
    private GameObject[] theme1_levels = new GameObject[10];
    [SerializeField]
    private GameObject[] theme2_levels = new GameObject[10];

    [SerializeField]
    private GameObject soundsPanel, profilePanel;
    [SerializeField]
    private Slider volume;

    [SerializeField]
    private GameObject[] themes = new GameObject[2];

    [SerializeField]
    private Button back_button, sounds, statistics, profile_back, profile_settings;

    [SerializeField]
    private GameObject profile_container, profile, username, theme_progress;
    [SerializeField]
    private Sprite[] avatar_container = new Sprite[10];
    [SerializeField]
    private Sprite[] avatar = new Sprite[10];
    [SerializeField]
    private Sprite[] username_container = new Sprite[10];

    [SerializeField]
    private GameObject[] profileDetails = new GameObject[9];
    [SerializeField]
    private GameObject[] profileLabels = new GameObject[4];
    [SerializeField]
    private Sprite[] fullAvatar = new Sprite[10];
    [SerializeField]
    private Sprite[] name_container = new Sprite[10];
    [SerializeField]
    private Sprite[] fullAvatar_container = new Sprite[10];
    [SerializeField]
    private Sprite[] profileDetails_container = new Sprite[10];
    [SerializeField]
    private Sprite[] fullProfile_container = new Sprite[10];

    [SerializeField]
    private Button[] numbers = new Button[12];
    [SerializeField]
    private TextMeshProUGUI inputField;
    [SerializeField]
    private GameObject restrictions;

    string[] colors = { "#5B2C6F", "#718F11", "#F33B3B", "#FF9C07", "#761500", "#7C1701", "#156784", "#115B31", "#7F2100", "#7C1701" };





    int user_id, guardian_id, current_theme, current_level, selected_theme, color_index;

    string user, gender, avatar_filename, relation_to_guardian;
    int age;

    void Start()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 1);
            volume.value = 1;
        }
        else
        {
             volume.value = PlayerPrefs.GetFloat("volume"); 
        }

        back_button.onClick.AddListener(() => GoToMap());
        sounds.onClick.AddListener(() => ShowSoundPanel());
        statistics.onClick.AddListener(() => ShowRestrictions());
        profile_back.onClick.AddListener(() => ShowProfilePanel());

        user_id = PlayerPrefs.GetInt("Current_user");
        guardian_id = PlayerPrefs.GetInt("Guardian_ID");
        current_theme = PlayerPrefs.GetInt("Current_theme");
        current_level = PlayerPrefs.GetInt("Current_level");
        selected_theme = PlayerPrefs.GetInt("Selected Theme");

        if (current_level == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(9);
        }

        for (int i = 0; i < theme1_levels.Length; i++)
        {
            Button level = theme1_levels[i].GetComponent<Button>();
            int index = i+1;
            level.onClick.AddListener(() => EnterGameLevel(index));
        }

        for (int i = 0; i < themes.Length; i++)
        {

            if (selected_theme == i+1)
            {
                themes[i].SetActive(true);
                if (current_theme > selected_theme)
                {
                    EnableAllLevels();
                }
                else
                {
                    EnableActiveLevels(current_level);
                }
            }
            else
            {
                themes[i].SetActive(false);
            }
        }

        UpdateUserProfile();

        for (int i = 0; i < numbers.Length; i++)
        {
            int index = i;
            numbers[i].onClick.AddListener(() => EnterNumber(numbers[index]));
        }
    }

    private void UpdateUserProfile()
    {
        StartCoroutine(getUser(user_id));
    }

    IEnumerator getUser(int user_id)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/users?ID=" + user_id))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Received: " + www.downloadHandler.text);
                UserRoot json = JsonConvert.DeserializeObject<UserRoot>(www.downloadHandler.text);
                user = json.data[0].username;
                gender = json.data[0].gender;
                age = json.data[0].age;
                avatar_filename = json.data[0].avatar_filename;
                relation_to_guardian = json.data[0].relation_to_guardian;
                ChangeProfileDetails();
            }
        }
    }

    private void ChangeProfileDetails()
    {
        for (int i = 0; i < avatar.Length; i++)
        {
            if (avatar_filename == avatar[i].name)
            {
                color_index = i;
                profile_container.GetComponent<Image>().sprite = avatar_container[i];
                profile.GetComponent<Image>().sprite = avatar[i];
                profile.GetComponent<Button>().onClick.AddListener(() => ShowProfilePanel());
                username.GetComponent<Image>().sprite = username_container[i];
                username.GetComponentInChildren<TMP_Text>().text = user.FirstCharacterToUpper();
                theme_progress.GetComponentInChildren<TMP_Text>().text = "Tema " + current_theme + ": " + "Antas " + current_level;
                profileDetails[5].GetComponent<Image>().sprite = fullAvatar[i];
                profileDetails[4].GetComponent<Image>().sprite = name_container[i];
                profileDetails[6].GetComponent<Image>().sprite = fullAvatar_container[i];
                profileDetails[7].GetComponent<Image>().sprite = profileDetails_container[i];
                profileDetails[8].GetComponent<Image>().sprite = fullProfile_container[i];
            }
        }

        profileDetails[0].GetComponent<TMP_Text>().text = age.ToString();
        profileDetails[1].GetComponent<TMP_Text>().text = gender.FirstCharacterToUpper();
        profileDetails[2].GetComponent<TMP_Text>().text = relation_to_guardian.FirstCharacterToUpper();
        profileDetails[3].GetComponent<TMP_Text>().text = PlayerPrefs.GetString("Email");
        profileDetails[4].GetComponent<TMP_InputField>().text = user.FirstCharacterToUpper();

        for (int i = 0; i < profileLabels.Length; i++)
        {
            Color colorFromHex;
            UnityEngine.ColorUtility.TryParseHtmlString(colors[color_index], out colorFromHex);
            profileLabels[i].GetComponent<TMP_Text>().color = colorFromHex;
        }
    }

    private void ShowSoundPanel()
    {
        if (soundsPanel.activeSelf ==  false)
        {
            soundsPanel.SetActive(true);
        }
        else
        {
            soundsPanel.SetActive(false);
        }
    }

    private void ShowProfilePanel()
    {
        if (profilePanel.activeSelf == false)
        {
            profilePanel.SetActive(true);
        }
        else
        {
            profilePanel.SetActive(false);
        }
    }

    private void ShowRestrictions()
    {
        if (restrictions.activeSelf == false)
        {
            restrictions.SetActive(true);
        }
        else
        {
            restrictions.SetActive(false);
        }
    }
    private void GoToMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);
    }

    private void EnableAllLevels()
    { 
        for (int i = 0; i < 10; i++)
        {
            if (selected_theme == 1)
            {

                theme1_levels[i].SetActive(true);
            }
            else if (selected_theme == 2)
            {
                theme2_levels[i].SetActive(true);
            }
        }
    }

    private void EnableActiveLevels(int level)
    {
        for (int i = 0; i < level; i++)
        {
            if (current_theme == 1)
            {
                theme1_levels[i].SetActive(true);
            }
            else if (current_theme == 2)
            {
                theme2_levels[i].SetActive(true);
            }
        }
    }

    private void EnterGameLevel(int level)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(level + 9);
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volume.value;
        PlayerPrefs.SetFloat("volume", volume.value);
    }

    private void EnterNumber(Button number)
    {
        if (number.name != "next-button" && number.name != "back-button")
        {

            if (inputField.text.Length == 4)
            {
                inputField.text = number.name;
                numbers[10].interactable = false;
            }
            else if (inputField.text.Length == 3)
            {
                inputField.text = inputField.text + number.name;
                numbers[10].interactable = true;
            }
            else
            {
                inputField.text = inputField.text + number.name;
                numbers[10].interactable = false;
            }
        }
        else if (number.name == "back-button")
        {
            inputField.text = "";
            ShowRestrictions();
        }
        else if (number.name == "next-button")
        {
            StartCoroutine(VerifyBirthYear(inputField.text));
            Debug.Log(inputField.text);
        }
    }

    IEnumerator VerifyBirthYear(string year)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", guardian_id);
        form.AddField("birth_year", year);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/users_guardian/verify_year", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Received: " + www.downloadHandler.text);
                VerificationRoot json = JsonConvert.DeserializeObject<VerificationRoot>(www.downloadHandler.text);
                if (json.data != "Birth year is correct")
                {
                    inputField.color = Color.red;
                    yield return new WaitForSeconds(1);
                    inputField.text = "";
                    inputField.color = Color.black;
                }
                else
                {
                    inputField.text = "";
                    UnityEngine.SceneManagement.SceneManager.LoadScene(8);
                }
            }
        }
    }
}
