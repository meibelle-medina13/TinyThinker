using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateAccount : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nicknameInputField;
    [SerializeField]
    private TextMeshProUGUI errorMessage;

    public void OnSubmitNewAccount()
    {
        string nickname = nicknameInputField.text;

        bool valid = ValidNickname(nickname);

        if (valid)
        {
            Debug.Log("Yey! Welcome " + nickname);
            PlayerPrefs.SetString("Name", nickname);
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
        else
        {
            Debug.Log("Please enter a nickname");
            errorMessage.text = "ERROR: Please enter a nickname";
        }
    }

    private bool ValidNickname(string nickname)
    {

        if (string.IsNullOrEmpty(nickname))
        {
            return false;
        } 
        else
        {
            return true;
        }
    }

    public void RemoveErrorMessage()
    {
        errorMessage.text = "";
    }
}
