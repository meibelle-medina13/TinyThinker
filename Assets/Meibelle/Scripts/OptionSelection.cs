using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json;

public class OptionSelection : MonoBehaviour
{
    ToggleGroup toggleGroup;
    RectTransform rectTransform;

    [SerializeField]
    private GameObject childname;
    [SerializeField]
    private TextMeshProUGUI age;
    [SerializeField]
    private TextMeshProUGUI gender;
    [SerializeField]
    private TextMeshProUGUI relationship;

    [SerializeField]
    private GameObject ageObject;
    [SerializeField]
    private GameObject genderObject;
    [SerializeField]
    private GameObject avatarObject;

    [SerializeField]
    public GameObject femaleObject;
    [SerializeField]
    public GameObject maleObject;

    [SerializeField]
    private GameObject mainmenuObject;

    [SerializeField]
    private Image profile;
    [SerializeField]
    private Image profileContainer;

    [SerializeField]
    private Sprite[] spritesFemale = new Sprite[6];
    [SerializeField]
    private Sprite[] spritesMale = new Sprite[6];


    string chosen = default;
    string nickname;
    string selected;
    string URL = "http://localhost:3000/users";
    int guardianID;


    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        nickname = PlayerPrefs.GetString("Name");
        childname.GetComponentInChildren<TMP_Text>().text = nickname;

        string email = PlayerPrefs.GetString("Email");
        Debug.Log(email);
        StartCoroutine(getGuardianID(email));
    }

    public void Next()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();

        chosen = toggle.GetComponentInChildren<TMP_Text>().text;
        if (string.IsNullOrEmpty(chosen))
        {
            for(int i = 0; i < 5; i++)
            {
                int num = i + 1;
                if (toggle.name == "male_" + num.ToString())
                {
                    profile.sprite = spritesMale[i];
                    profileContainer.sprite = spritesMale[5];
                    PlayerPrefs.SetString("Avatar", spritesMale[i].name);
                }
                else if (toggle.name == "female_" + num.ToString())
                {
                    profile.sprite = spritesFemale[i];
                    profileContainer.sprite = spritesFemale[5];
                    PlayerPrefs.SetString("Avatar", spritesFemale[i].name);
                }
            }

            avatarObject.SetActive(false);
            mainmenuObject.SetActive(true);
            age.text = "EDAD: " + PlayerPrefs.GetString("Age");
            gender.text = "KASARIAN: " + PlayerPrefs.GetString("Gender");
            relationship.text = "KAUGNAYAN: " + PlayerPrefs.GetString("Relationship");

            rectTransform = childname.GetComponent<RectTransform>();
            rectTransform.transform.localPosition = new Vector3(0, -10, 0);

        }
        else
        {
            if (chosen == "LALAKI" || chosen == "BABAE")
            {
                
                genderObject.SetActive(false);
                avatarObject.SetActive(true);
                if (chosen == "BABAE")
                {
                    femaleObject.SetActive(true);
                }
                else
                {
                    maleObject.SetActive(true);
                }
                PlayerPrefs.SetString("Gender", chosen);
            }
            else
            {
                ageObject.SetActive(false);
                genderObject.SetActive(true);
                PlayerPrefs.SetString("Age", chosen);
                
            }

            Debug.Log(toggle.name);
        }
    }

    public void RegisterAccount()
    {
        Regex pattern = new Regex("(\\d{1})");
        Match match = pattern.Match(PlayerPrefs.GetString("Age"));
        Debug.Log(match.Value);

        int current_theme = 1;
        int current_level = 0;
        int age;
        int.TryParse(match.Value, out age);
        string gender = PlayerPrefs.GetString("Gender");
        string avatar_filename = PlayerPrefs.GetString("Avatar");

        string relationship = PlayerPrefs.GetString("Relationship");
        ;
        
        StartCoroutine(Upload(nickname, age, gender, avatar_filename, current_theme, current_level, relationship, guardianID));
    }

    IEnumerator Upload(string username, int age, string gender, string avatar_filename, int current_theme, int current_level, string relation_to_guardian, int guardian_ID)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("age", age);
        form.AddField("gender", gender);
        form.AddField("avatar_filename", avatar_filename);
        form.AddField("current_theme", current_theme);
        form.AddField("current_level", current_level);
        form.AddField("relation_to_guardian", relation_to_guardian);
        form.AddField("guardian_ID", guardian_ID);

        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Received: " + www.downloadHandler.text);
                UnityEngine.SceneManagement.SceneManager.LoadScene(5);
            }
        }
    }

    [Serializable]
    public class Datum
    {
        public int ID { get; set; }
    }

    [Serializable]
    public class Root
    {
        public bool success { get; set; }
        public List<Datum> data { get; set; }
    }

    IEnumerator getGuardianID(string email)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/users_guardian/guardianID?email="+email))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Received: " + www.downloadHandler.text);
                Root json = JsonConvert.DeserializeObject<Root>(www.downloadHandler.text);
                guardianID = json.data[0].ID;
                Debug.Log(json.data[0].ID);
            }
        }
    }
}
