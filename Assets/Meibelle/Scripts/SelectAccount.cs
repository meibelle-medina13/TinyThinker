using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SelectAccount : MonoBehaviour
{
    [Header("<---- PARENT GAMEOBJECT ---->")]
    [SerializeField]
    private GameObject parentGameObject;

    [Header("<---- ADD ACCOUNT ---->")]
    [SerializeField]
    private Button AddAccount;

    [Header("<---- AVATAR/CONTAINER SPRITES ---->")]
    [SerializeField]
    private Sprite[] avatars = new Sprite[10];
    [SerializeField]
    private Sprite[] account_container = new Sprite[10];

    [Header("<---- USER TEMPLATE ---->")]
    public GameObject user;

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private SELECT_ACCOUNT_REQUESTS requestsManager;

    private string[] username;
    private string[] gender;
    private string[] avatar_filename;
    private int[] user_id;
    private int[] current_theme;
    private int[] current_level;
    private int no_user;
    

    private void Start()
    {
        requestsManager = FindObjectOfType<SELECT_ACCOUNT_REQUESTS>();

        AddAccount.onClick.AddListener(() => NewAccount());

        string email = PlayerPrefs.GetString("Email");
        StartCoroutine(GetAllUsers(email));
    }

    void DisplayUsers(int num)
    {
        int y = 350;
        for (int i = 0; i < avatars.Length; i++)
        {
            if (avatars[i].name == avatar_filename[0])
            {
                user.GetComponentInChildren<SpriteRenderer>().sprite = account_container[i];
                user.GetComponentInChildren<SpriteRenderer>().GetComponentInChildren<Canvas>().GetComponentInChildren<SpriteRenderer>().sprite = avatars[i];
                user.GetComponentInChildren<SpriteRenderer>().GetComponentInChildren<TMP_Text>().text = username[0].ToUpper();
                user.GetComponent<Button>().onClick.AddListener(() => LogIn(0));
            }
        }
        for (int i = 1; i < num; i++)
        {
            GameObject Clone = Instantiate(user);
            y = y - 50;
            Clone.transform.position = new Vector3(394f, y, 0f);
            Clone.transform.SetParent(parentGameObject.transform);
            int index = i;

            for (int j = 0; j < avatars.Length; j++)
            {
                if (avatars[j].name == avatar_filename[i])
                {
                    Clone.GetComponentInChildren<SpriteRenderer>().sprite = account_container[j];
                    Clone.GetComponentInChildren<SpriteRenderer>().GetComponentInChildren<Canvas>().GetComponentInChildren<SpriteRenderer>().sprite = avatars[j];
                    Clone.GetComponentInChildren<SpriteRenderer>().GetComponentInChildren<Canvas>().GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(39, 32, 33);
                    Clone.GetComponentInChildren<SpriteRenderer>().GetComponentInChildren<Canvas>().GetComponentInChildren<SpriteRenderer>().transform.localPosition = new Vector3(-495, -350, 90);
                    Clone.GetComponentInChildren<SpriteRenderer>().GetComponentInChildren<TMP_Text>().text = username[i].ToUpper();
                    Clone.GetComponent<Button>().onClick.AddListener(() => LogIn(index));
                }
            }
        }
    }

    private void LogIn(int index)
    {
        PlayerPrefs.SetInt("Current_user", user_id[index]);
        PlayerPrefs.SetInt("Current_theme", current_theme[index]);
        PlayerPrefs.SetInt("Current_level", current_level[index]);

        if (current_theme[index] == 1 && current_level[index] == 0)
        {
            if (!PlayerPrefs.HasKey("StartGuide" + user_id[index].ToString()))
            {
                PlayerPrefs.SetString("StartGuide" + user_id[index].ToString(), "True");
                PlayerPrefs.SetFloat(user_id[index].ToString() + "Time", 7200);
            }
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(6);
    }

    private void NewAccount()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

    IEnumerator GetAllUsers(string email)
    {
        yield return StartCoroutine(requestsManager.GetGuardianID("/users_guardian/guardianID", email));
        if (requestsManager.guardianID != 0)
        {
            yield return StartCoroutine(requestsManager.GetUsers("/users", requestsManager.guardianID));

            if (requestsManager.json != null)
            {
                no_user = requestsManager.json.data.Count;
                username = new string[no_user];
                gender = new string[no_user];
                user_id = new int[no_user];
                avatar_filename = new string[no_user];
                current_theme = new int[no_user];
                current_level = new int[no_user];

                for (int i = 0; i < no_user; i++)
                {
                    username[i] = requestsManager.json.data[i].username;
                    gender[i] = requestsManager.json.data[i].gender;
                    user_id[i] = requestsManager.json.data[i].ID;
                    avatar_filename[i] = requestsManager.json.data[i].avatar_filename;
                    current_theme[i] = requestsManager.json.data[i].current_theme;
                    current_level[i] = requestsManager.json.data[i].current_level;
                }
                DisplayUsers(no_user);
            }
        }
    }
}
