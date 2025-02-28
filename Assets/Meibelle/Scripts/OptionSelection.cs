using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.Text.RegularExpressions;

public class OptionSelection : MonoBehaviour
{
    ToggleGroup toggleGroup;
    RectTransform rectTransform;

    [Header("<---- CHILD DETAILS ---->")]
    [SerializeField]
    private GameObject childname;
    [SerializeField]
    private TextMeshProUGUI age;
    [SerializeField]
    private TextMeshProUGUI gender;
    [SerializeField]
    private TextMeshProUGUI relationship;

    [Header("<---- OPTIONS PANEL ---->")]
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

    [Header("<---- CONFIRMATION PANEL ---->")]
    [SerializeField]
    private GameObject mainmenuObject;

    [Header("<---- PROFILE IMAGE ---->")]
    [SerializeField]
    private Image profile;
    [SerializeField]
    private Image profileContainer;

    [Header("<---- AVATAR SPRITES ---->")]
    [SerializeField]
    private Sprite[] spritesFemale = new Sprite[6];
    [SerializeField]
    private Sprite[] spritesMale = new Sprite[6];

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private SETUP_REQUESTS requestsManager;

    private string chosen = default;
    private string nickname;

    void Start()
    {
        requestsManager = FindObjectOfType<SETUP_REQUESTS>();

        toggleGroup = GetComponent<ToggleGroup>();
        nickname = PlayerPrefs.GetString("Name");
        childname.GetComponentInChildren<TMP_Text>().text = nickname;

        string email = PlayerPrefs.GetString("Email");
        StartCoroutine(requestsManager.getGuardianID("/users_guardian/guardianID", email));
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
        }
    }

    public void RegisterAccount()
    {
        Regex pattern = new Regex("(\\d{1})");
        Match match = pattern.Match(PlayerPrefs.GetString("Age"));

        int current_theme = 1;
        int current_level = 0;
        int age;
        int.TryParse(match.Value, out age);
        string gender = PlayerPrefs.GetString("Gender");
        string avatar_filename = PlayerPrefs.GetString("Avatar");
        string relationship = PlayerPrefs.GetString("Relationship");

        StartCoroutine(requestsManager.AddUser("/users", nickname, age, gender, avatar_filename, current_theme, current_level, relationship));
    }
}
