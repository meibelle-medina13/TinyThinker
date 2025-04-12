using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class EditProfile : MonoBehaviour
{
    [Header("<---- CHANGE AVATAR GAMEOBJECT ---->")]
    [SerializeField]
    private GameObject changeAvatarPanel;

    [Header("<---- GENDER CLASSIFICATION AVATARS ---->")]
    [SerializeField]
    private GameObject[] avatarOptions = new GameObject[2];

    [Header("<---- AVATAR BUTTONS ---->")]
    [SerializeField]
    private Button[] femaleOptions = new Button[5];
    [SerializeField]
    private Button[] maleOptions = new Button[5];

    //[Header("<---- PROFILE ICON DETAILS SPRITES ---->")]
    //[SerializeField]
    //private Sprite[] avatar_container = new Sprite[10];
    //[SerializeField]
    //private Sprite[] avatar = new Sprite[10];
    //[SerializeField]
    //private Sprite[] username_container = new Sprite[10];

    [Header("<---- FULL PROFILE DETAILS SPRITES ---->")]
    [SerializeField]
    private Sprite[] fullAvatarSprites = new Sprite[10];
    [SerializeField]
    private Sprite[] name_containerSprites = new Sprite[10];
    [SerializeField]
    private Sprite[] fullAvatar_containerSprites = new Sprite[10];
    [SerializeField]
    private Sprite[] profileDetails_containerSprites = new Sprite[10];
    [SerializeField]
    private Sprite[] fullProfile_containerSprites = new Sprite[10];

    [Header("<---- FULL PROFILE DETAILS GAMEOBJECT ---->")]
    [SerializeField]
    private Image fullAvatar;
    [SerializeField]
    private Image nameContainer;
    [SerializeField]
    private Image fullAvatarContainer;
    [SerializeField]
    private Image profileDetailsContainer;
    [SerializeField]
    private Image fullProfileContainer;

    [Header("<---- BACK/SUBMIT/CANCEL BUTTON ---->")]
    [SerializeField]
    private Button saveButton;
    [SerializeField]
    private Button submitButton;
    [SerializeField]
    private Button cancelButton;

    [Header("<---- EDIT PROFILE ---->")]
    [SerializeField]
    private Button editAvatar;
    [SerializeField]
    private Button editName;
    [SerializeField]
    private GameObject nameField;
    [SerializeField]
    private Sprite editNameContainer;

    [Header("<---- REQUEST SCRIPT ---->")]
    [SerializeField]
    private LEVEL_MAP_REQUESTS requestsManager;


    private Sprite oldFullAvatarSprite;
    private Sprite oldNameContainer;
    private Sprite oldFullAvatarContainer;
    private Sprite oldProfileDetailsContainer;
    private Sprite oldFullProfileContainer;
    private Sprite prevNameContainer;
    private string oldUsername, oldAvatar;

    private string avatar_filename, gender, username, newAvatar_filename;

    private void Start()
    {
        requestsManager = FindObjectOfType<LEVEL_MAP_REQUESTS>();

        avatar_filename = PlayerPrefs.GetString("Avatar");
        string[] letterArray = avatar_filename.Split('_');
        gender = letterArray[0];

        Debug.Log(letterArray[0]);

        StoreOriginalProfile();

        if (gender == "female")
        {
            avatarOptions[0].SetActive(true);
            avatarOptions[1].SetActive(false);

            foreach (Button avatar in femaleOptions)
            {
                avatar.onClick.AddListener(() => SelectAvatar(avatar));
            }
        }
        else if (gender == "male")
        {
            avatarOptions[1].SetActive(true);
            avatarOptions[0].SetActive(false);

            foreach (Button avatar in maleOptions)
            {
                avatar.onClick.AddListener(() => SelectAvatar(avatar));
            }
        }
        editName.gameObject.SetActive(true);
        cancelButton.onClick.AddListener(() => RetainOldProfile());
        editAvatar.onClick.AddListener(() => AvatarChoicesPopup());
        editName.onClick.AddListener(() => EditName());
        saveButton.onClick.AddListener(() => StartCoroutine(SaveAllChanges()));
    }

    private void AvatarChoicesPopup()
    {
        if (changeAvatarPanel.activeSelf)
        {
            changeAvatarPanel.SetActive(false);
        }
        else
        {
            changeAvatarPanel.SetActive(true);
            submitButton.onClick.AddListener(() => AvatarChoicesPopup());
        }

        Debug.Log("Popup" + username + " " + newAvatar_filename);
        if ((string.IsNullOrEmpty(newAvatar_filename) || newAvatar_filename == oldAvatar) && (string.IsNullOrEmpty(username) || username == oldUsername))
        {
            saveButton.interactable = false;
        }
        else
        {
            saveButton.interactable = true;
        }
    }

    private void EditName()
    {
        editName.gameObject.SetActive(false);
        prevNameContainer = nameContainer.sprite;
        nameField.GetComponent<TMP_InputField>().interactable = true;
        nameContainer.sprite = editNameContainer;
        saveButton.interactable = true;
    }

    private void StoreOriginalProfile()
    {
        oldUsername = nameField.GetComponent<TMP_InputField>().text;
        oldAvatar = (fullAvatar.sprite.name.Replace("-", "_"));
        oldFullAvatarSprite = fullAvatar.sprite;
        oldNameContainer = nameContainer.sprite;
        oldFullAvatarContainer = fullAvatarContainer.sprite;
        oldProfileDetailsContainer = profileDetailsContainer.sprite;
        oldFullProfileContainer = fullProfileContainer.sprite;
        Debug.Log(oldUsername);
    }

    private void RetainOldProfile()
    {
        nameField.GetComponent<TMP_InputField>().text = oldUsername;
        fullAvatar.sprite = oldFullAvatarSprite;
        nameContainer.sprite = oldNameContainer;
        fullAvatarContainer.sprite = oldFullAvatarContainer;
        profileDetailsContainer.sprite = oldProfileDetailsContainer;
        fullProfileContainer.sprite = oldFullProfileContainer;
        newAvatar_filename = "";
        saveButton.interactable = false;
    }

    private void SelectAvatar(Button selectedAvatar)
    {
        int index, avatar_num, spriteIndex;
        string[] avatar_name = selectedAvatar.name.Split("_");
        avatar_num = int.Parse(avatar_name[1]);

        if (gender == "female")
        {
            index = 0;
        }
        else
        {
            index = 5;
        }

        spriteIndex = (avatar_num + index) - 1;
        
        fullAvatar.sprite = fullAvatarSprites[spriteIndex];
        nameContainer.sprite = name_containerSprites[spriteIndex];
        fullAvatarContainer.sprite = fullAvatar_containerSprites[spriteIndex];
        profileDetailsContainer.sprite = profileDetails_containerSprites[spriteIndex];
        fullProfileContainer.sprite = fullProfile_containerSprites[spriteIndex];

        newAvatar_filename = fullAvatar.sprite.name.Replace("-", "_");
    }

    IEnumerator SaveAllChanges()
    {
        int userID = PlayerPrefs.GetInt("Current_user");
        username = nameField.GetComponent<TMP_InputField>().text;
        if (string.IsNullOrEmpty(username))
        {
            username = oldUsername;
        }
        
        if (string.IsNullOrEmpty(newAvatar_filename))
        {
            newAvatar_filename = oldAvatar;
        }
        yield return StartCoroutine(requestsManager.UpdateProfile("/users/updateProfile", userID, newAvatar_filename, username));

        AsyncOperation asyncOperation;
        asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(7);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
