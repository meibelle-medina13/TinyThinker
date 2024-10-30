using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

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
    private Sprite[] spritesFemale = new Sprite[5];
    [SerializeField]
    private Sprite[] spritesMale = new Sprite[5];


    string chosen = default;
    public string nickname;
    string selected;

    
    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        nickname = PlayerPrefs.GetString("Name");
        Debug.Log(nickname);
        childname.GetComponentInChildren<TMP_Text>().text = nickname;
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
                    Debug.Log(i + 1);
                }
                else if (toggle.name == "female_" + num.ToString())
                {
                    profile.sprite = spritesFemale[i];
                    Debug.Log(i + 1);
                }
            }

            avatarObject.SetActive(false);
            mainmenuObject.SetActive(true);
            age.text = "EDAD: " + PlayerPrefs.GetString("Age");
            gender.text = "KASARIAN: " + PlayerPrefs.GetString("Gender");

            rectTransform = childname.GetComponent<RectTransform>();
            rectTransform.transform.localPosition = new Vector3(0, -10, 0);

        }
        else
        {
            if (chosen == "MALE" || chosen == "FEMALE")
            {
                
                genderObject.SetActive(false);
                avatarObject.SetActive(true);
                if (chosen == "FEMALE")
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
                
                //Debug.Log(rectTransform.transform.localPosition);
            }

            Debug.Log(toggle.name);
        }
    }
}
