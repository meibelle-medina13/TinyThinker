using UnityEngine;
using UnityEngine.UI;

public class Button_Manager : MonoBehaviour
{ 
    public Button[] buttons;
    public Button correctButton;
    [Header("Highest to Lowest Score of Buttons")]
    public Button[] scoreCounter;
    public Button activate_button;
    public int score = 0;

    void Start()
    {
        activate_button.gameObject.SetActive(false);

        foreach (Button button in buttons)
        {
            Button capturedButton = button;
            capturedButton.onClick.AddListener(() => CheckButton(capturedButton));
        }
    }

    void DisableButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    void CheckButton(Button clickedButton)
    {
        Debug.Log("Button clicked: " + clickedButton.name);
        activate_button.gameObject.SetActive(true);
        DisableButtons();

        if (clickedButton == correctButton)
        {
            score += 3;
            Debug.Log("Correct button clicked!");
        }
        else if (clickedButton == scoreCounter[0])
        {
            score += 2;
            Debug.Log("Wrong button clicked.");
        }
        else if (clickedButton == scoreCounter[1])
        {
            score += 1;
            Debug.Log("Wrong button clicked.");
        }
    }
}
