using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class RotateHandler : MonoBehaviour
{
    public Button rotateButton;
    public float CorrectAngle = 2;
    public GameObject CorrectImage;
    public float rotationAngle = 90f;
    public float angleCounter = 0;
    public AudioSource RotateCorrectlySound;
    public AudioSource ForEveryRotateSound;
    private Theme1Level3_SceneManager sceneManagerL1_3;

    private void Start()
    {
        sceneManagerL1_3 = FindObjectOfType<Theme1Level3_SceneManager>();
        rotateButton.onClick.AddListener(Rotate);
    }

    public void Rotate()
    {
        transform.Rotate(0f, 0f, rotationAngle);
        angleCounter++;
        Debug.Log(angleCounter);

        if (angleCounter == CorrectAngle)
        {
            if (RotateCorrectlySound != null)
            {
                RotateCorrectlySound.Play();
            }

            gameObject.SetActive(false);
            CorrectImage.SetActive(true);
            sceneManagerL1_3.correctPuzzle++;
            correctAnswer_Checker();
        }
        else
        {
            ForEveryRotateSound.Play();
        }
    }

    public void correctAnswer_Checker()
    {
        if (sceneManagerL1_3.correctPuzzle == 4)
        {
            sceneManagerL1_3.correctPuzzle = 0;
            sceneManagerL1_3.fixedPuzzle++;
            sceneManagerL1_3.nextScene_Button.gameObject.SetActive(true);
        }

        if (sceneManagerL1_3.fixedPuzzle == 2)
        {
            sceneManagerL1_3.Smallconfetti.SetActive(true);
            //sceneManagerL1_3.nextScene_Button.gameObject.SetActive(true);
        }
    }
}

