using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Theme1_Map : MonoBehaviour
{
    [Header("<---- THEME LEVEL BUTTONS ---->")]
    [SerializeField]
    private GameObject[] theme1_levels = new GameObject[10];
    [SerializeField]
    private GameObject[] theme2_levels = new GameObject[10];
    [SerializeField]
    private GameObject[] theme3_levels = new GameObject[10];
    [SerializeField]
    private GameObject[] theme4_levels = new GameObject[10];

    [Header("<---- SOUNDS AND PROFILE PANEL ---->")]
    [SerializeField]
    private GameObject soundsPanel;
    [SerializeField]
    private GameObject profilePanel;
    [SerializeField]
    private Slider volume;

    [Header("<---- THEMES ---->")]
    [SerializeField]
    private GameObject[] themes_map = new GameObject[4];

    [Header("<---- BUTTONS ---->")]
    [SerializeField]
    private Button back_button;
    [SerializeField]
    private Button sounds;
    [SerializeField]
    private Button statistics;
    [SerializeField]
    private Button profile_back;
    [SerializeField]
    private Button profile_settings;


    [Header("<---- PROFILE ICON ---->")]
    [SerializeField]
    private GameObject profile_container;
    [SerializeField]
    private GameObject profile;
    [SerializeField]
    private GameObject username;
    [SerializeField]
    private GameObject theme_progress;

    [Header("<---- PROFILE DETAILS SPRITES ---->")]
    [SerializeField]
    private Sprite[] avatar_container = new Sprite[10];
    [SerializeField]
    private Sprite[] avatar = new Sprite[10];
    [SerializeField]
    private Sprite[] username_container = new Sprite[10];
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

    [Header("<---- PROFILE DETAILS ---->")]
    [SerializeField]
    private GameObject[] profileDetails = new GameObject[10];
    [SerializeField]
    private GameObject[] profileLabels = new GameObject[4];

    [Header("<---- RESTRICTIONS ---->")]
    [SerializeField]
    private Button[] numbers = new Button[12];
    [SerializeField]
    private TextMeshProUGUI inputField;
    [SerializeField]
    private GameObject restrictions;

    [Header("<---- EDIT PROFILE GAMEOBJECT ---->")]
    [SerializeField]
    private Sprite editNameContainer;
    [SerializeField]
    private GameObject editProfile;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button cancel;

    [Header("<---- LOADING PANEL ---->")]
    [SerializeField]
    private GameObject INLoadingScene;
    [SerializeField]
    private GameObject OUTLoadingScene;

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private LEVEL_MAP_REQUESTS requestsManager;

    private string[] colors = { "#5B2C6F", "#718F11", "#F33B3B", "#FF9C07", "#761500", "#7C1701", "#156784", "#115B31", "#7F2100", "#7C1701" };
    private int user_id, guardian_id, current_theme, current_level, selected_theme, color_index, age;
    private string user, gender, avatar_filename, relation_to_guardian;

    void Start()
    {
        OUTLoadingScene.SetActive(false);
        StartCoroutine(INLoading());

        requestsManager = FindObjectOfType<LEVEL_MAP_REQUESTS>();

        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1);
            volume.value = 1;
        }
        else
        {
             volume.value = PlayerPrefs.GetFloat("Volume"); 
        }

        back_button.onClick.AddListener(() => GoTo_ThemeSelection());
        sounds.onClick.AddListener(() => ShowPopUpPanel("sounds"));
        statistics.onClick.AddListener(() => ShowPopUpPanel("statistics"));
        profile_back.onClick.AddListener(() => ShowPopUpPanel("profile"));

        user_id = PlayerPrefs.GetInt("Current_user");
        guardian_id = PlayerPrefs.GetInt("Guardian_ID");
        current_theme = PlayerPrefs.GetInt("Current_theme");
        current_level = PlayerPrefs.GetInt("Current_level");
        selected_theme = PlayerPrefs.GetInt("Selected_theme");
        CheckingThemeLevel();

        StartCoroutine(UpdateUserProfile());

        for (int i = 0; i < numbers.Length; i++)
        {
            int index = i;
            numbers[i].onClick.AddListener(() => EnterNumber(numbers[index]));
        }
    }

    private void CheckingThemeLevel()
    {
        for (int i = 0; i < themes_map.Length; i++)
        {

            if (selected_theme == i + 1)
            {
                themes_map[i].SetActive(true);
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
                themes_map[i].SetActive(false);
            }
        }

        int theme = 0;
        if (selected_theme == 1)
        {
            theme = 1;
            if (current_level == 0 && current_theme == 1)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(9);
            }
            else if (current_level == 5 && current_theme == 1)
            {
                if (PlayerPrefs.HasKey("PostTest Status") && PlayerPrefs.GetString("PostTest Status") == "Not yet done")
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(15);
                }
            }

            for (int i = 0; i < theme1_levels.Length; i++)
            {
                Button level = theme1_levels[i].GetComponent<Button>();
                int levelnum = i + 1;
                level.onClick.AddListener(() => EnterGameLevel(levelnum, theme));
            }
        }
        else if (selected_theme == 2)
        {
            Debug.Log(current_level);
            theme = 2;
            if (current_level == 0 && current_theme == 2)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(16);
            }
            else if (current_level == 5 && current_theme == 2)
            {
                if (PlayerPrefs.HasKey("PostTest Status") && PlayerPrefs.GetString("PostTest Status") == "Not yet done")
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(15);
                }
            }

            for (int i = 0; i < theme2_levels.Length; i++)
            {
                Button level = theme2_levels[i].GetComponent<Button>();
                int levelnum = i + 1;
                level.onClick.AddListener(() => EnterGameLevel(levelnum, theme));
            }
        }
        else if (selected_theme == 3)
        {
            Debug.Log(current_level);
            theme = 3;
            if (current_level == 0 && current_theme == 3)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(23);
            }
            else if (current_level == 3 && current_theme == 3)
            {
                if (PlayerPrefs.HasKey("PostTest Status") && PlayerPrefs.GetString("PostTest Status") == "Not yet done")
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(27);
                }
            }

            for (int i = 0; i < theme3_levels.Length; i++)
            {
                Button level = theme3_levels[i].GetComponent<Button>();
                int levelnum = i + 1;
                level.onClick.AddListener(() => EnterGameLevel(levelnum, theme));
            }
        }
        else if (selected_theme == 4)
        {
            Debug.Log(current_level);
            theme = 4;
            if (current_level == 0 && current_theme == 4)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(28);
            }
            else if (current_level == 3 && current_theme == 3)
            {
                if (PlayerPrefs.HasKey("PostTest Status") && PlayerPrefs.GetString("PostTest Status") == "Not yet done")
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(32);
                }
            }

            for (int i = 0; i < theme4_levels.Length; i++)
            {
                Button level = theme4_levels[i].GetComponent<Button>();
                int levelnum = i + 1;
                level.onClick.AddListener(() => EnterGameLevel(levelnum, theme));
            }
        }
    }

    IEnumerator INLoading()
    {
        yield return new WaitForSeconds(2f);
        INLoadingScene.SetActive(false);
    }

    IEnumerator OUTLoading(int sceneIndex)
    {
        OUTLoadingScene.SetActive(true);
        yield return new WaitForSeconds(3f);

        AsyncOperation asyncOperation;
        asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
    private void ShowPopUpPanel(string actionButton)
    {
        GameObject currentPanel = null;
        if (actionButton == "sounds")
        {
            currentPanel = soundsPanel;
        }
        else if (actionButton == "statistics")
        {
            currentPanel = restrictions;
        }
        else if (actionButton == "profile")
        {
            currentPanel = profilePanel;
            settingsButton.onClick.AddListener(() => EnableEditProfile());
        }

        if (currentPanel.activeSelf == false)
        {
            currentPanel.SetActive(true);
        }
        else
        {
            currentPanel.SetActive(false);
        }
    }

    private void EnableEditProfile()
    {
        settingsButton.gameObject.SetActive(false);
        editProfile.SetActive(true);
        profile_back.gameObject.SetActive(false);
        cancel.onClick.AddListener(() => DisableEditProfile());
    }

    private void DisableEditProfile()
    {
        editProfile.SetActive(false);
        settingsButton.gameObject.SetActive(true);
        profile_back.gameObject.SetActive(true);
    }

    IEnumerator UpdateUserProfile()
    {
        yield return StartCoroutine(requestsManager.GetUser("/users", user_id));

        if (requestsManager.json != null)
        {
            user = requestsManager.json.data[0].username;
            gender = requestsManager.json.data[0].gender;
            age = requestsManager.json.data[0].age;
            avatar_filename = requestsManager.json.data[0].avatar_filename;
            relation_to_guardian = requestsManager.json.data[0].relation_to_guardian;
            ChangeProfileDetails();
        }
    }

    private void ChangeProfileDetails()
    {
        for (int i = 0; i < avatar.Length; i++)
        {
            if (avatar_filename == avatar[i].name)
            {
                PlayerPrefs.SetString("Avatar", avatar[i].name);
                color_index = i;
                profile_container.GetComponent<Image>().sprite = avatar_container[i];
                profile.GetComponent<Image>().sprite = avatar[i];
                profile.GetComponent<Button>().onClick.AddListener(() => ShowPopUpPanel("profile"));
                username.GetComponent<Image>().sprite = username_container[i];
                username.GetComponentInChildren<TMP_Text>().text = user.FirstCharacterToUpper();
                theme_progress.GetComponentInChildren<TMP_Text>().text = "Tema " + current_theme + ": " + "Antas " + current_level;
                profileDetails[5].GetComponent<Image>().sprite = fullAvatar[i];
                profileDetails[9].GetComponent<Image>().sprite = name_container[i];
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

    private void GoTo_ThemeSelection()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);
    }

    private void EnableAllLevels()
    { 
        for (int i = 0; i < 5; i++)
        {
            if (selected_theme == 1)
            {

                theme1_levels[i].SetActive(true);
            }
            else if (selected_theme == 2)
            {
                theme2_levels[i].SetActive(true);
            }
            else if (selected_theme == 3 && i < 3)
            {
                theme3_levels[i].SetActive(true);
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
            else if (current_theme == 3)
            {
                theme3_levels[i].SetActive(true);
            }
            else if (current_theme == 4)
            {
                theme4_levels[i].SetActive(true);
            }
        }
    }

    private void EnterGameLevel(int level, int theme)
    {
        int indexFiller = 0;
        if (theme == 1)
        {
            indexFiller = 9;
        }
        else if (theme == 2)
        {
            indexFiller = 16;
        }
        else if (theme == 3)
        {
            indexFiller = 23;
        }
        else if (theme == 4)
        {
            indexFiller = 28;
        }
        Debug.Log(indexFiller);
        //UnityEngine.SceneManagement.SceneManager.LoadScene(level + indexFiller);
        StartCoroutine(OUTLoading(level + indexFiller));
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volume.value;
        PlayerPrefs.SetFloat("Volume", volume.value);
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
            ShowPopUpPanel("statistics");
        }
        else if (number.name == "next-button")
        {
            StartCoroutine(ValidateBirthYear());
        }
    }

    IEnumerator ValidateBirthYear()
    {
        yield return StartCoroutine(requestsManager.VerifyBirthYear("/users_guardian/verify_year", inputField.text, guardian_id));

        if (requestsManager.verificationJson.data != "Birth year is correct")
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
