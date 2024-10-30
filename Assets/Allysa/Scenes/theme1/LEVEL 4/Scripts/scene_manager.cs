using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public int counter = 1;
    public List<GameObject> scenes;
    public AudioSource Background_music1;
    public AudioSource Background_music2;
    //public GameObject assessment;
    //public GameObject achievementboard;

    //change background
    [Header("Change Background")]
    public Image panelImage;
    public List<Sprite> newImage;

    //activate button
    [Header("Activate Buttons")]

    [Header("Scene 8")]
    [Header("Buttons to activate")]
    public List<Button> SCENE8_activate_buttons;
    [Header("Buttons to deactivate")]
    public List<GameObject> SCENE8_deactivate_buttons;

    [Header("Scene 9")]
    [Header("Button to activate")]
    public GameObject SCENE9_activate_button;
    [Header("Button to deactivate")]
    public GameObject SCENE9_deactivate_button;

    [Header("Assessment")]
    [Header("Buttons to activate")]
    public List<Button> ASSESSMENT_activate_buttons;
    [Header("Buttons to deactivate")]
    public List<GameObject> ASSESSMENT_deactivate_buttons;

    [Header("Next Button")]
    public Button nextButton;

    void Start()
    {
        Background_music1.Play();
    }

    void Update()
    {
        if (counter == 10)
        {
            if (Background_music1.isPlaying)
            {
                Background_music1.Stop();
            }
            if (!Background_music2.isPlaying)
            {
                Background_music2.Play();
            }
        }
    }

    public void whenButtonClicked()
    {
        if (counter <= 10)
        {
            if (counter == 1)
            {
                scenes[counter].SetActive(false);
                counter++;
                scenes[counter].SetActive(true);
                nextButton.gameObject.SetActive(false);
            }

            else if (counter == 3)
            {
                panelImage.sprite = newImage[2];
                scenes[counter].SetActive(false);
                counter++;
                scenes[counter].SetActive(true);
                nextButton.gameObject.SetActive(false);
            }

            else if (counter == 6)
            {
                panelImage.sprite = newImage[3];
                scenes[counter].SetActive(false);
                counter++;
                scenes[counter].SetActive(true);
                deactivate_buttons();
                nextButton.gameObject.SetActive(false);
            }

            else if (counter == 7)
            {
                panelImage.sprite = newImage[4];
                scenes[counter].SetActive(false);
                counter++;
                scenes[counter].SetActive(true);
                deactivate_buttons();
                nextButton.gameObject.SetActive(false);

            }

            //else if (counter == 10)
            //{
            //    assessment.SetActive(false);
            //    achievementboard.SetActive(true);
            //}

            else
            {
                scenes[counter].SetActive(false);
                counter++;
                scenes[counter].SetActive(true);
                nextButton.gameObject.SetActive(false);
            }
        }
    }

    public void OnAnimationEnd()
    {
        if (counter == 1)
        {
            scenes[counter].SetActive(false);
            counter++;
            scenes[counter].SetActive(true);
            panelImage.sprite = newImage[0];
        }

        else if (counter == 2)
        {
            scenes[counter].SetActive(false);
            counter++;
            scenes[counter].SetActive(true);
            panelImage.sprite = newImage[1];
        }

        else if (counter == 7 )
        {
            foreach (Button button in SCENE8_activate_buttons)
            {
                button.gameObject.SetActive(true);
            }

            foreach (GameObject deact_button in SCENE8_deactivate_buttons)
            {
                deact_button.gameObject.SetActive(false);
            }
        }

        else if (counter == 8)
        {
            SCENE9_deactivate_button.SetActive(false);
            SCENE9_activate_button.SetActive(true);
        }

        else if (counter == 10)
        {
            foreach (Button button in ASSESSMENT_activate_buttons)
            {
                button.gameObject.SetActive(true);
            }

            foreach (GameObject deact_button in ASSESSMENT_deactivate_buttons)
            {
                deact_button.gameObject.SetActive(false);
            }
        }

        else
        {
            nextButton.gameObject.SetActive(true);
        }
    }

    void deactivate_buttons()
    {
        foreach (Button button in SCENE8_activate_buttons)
        {
            button.gameObject.SetActive(false);
        }

        foreach (Button button in ASSESSMENT_activate_buttons)
        {
            button.gameObject.SetActive(false);
        }
    }
}
