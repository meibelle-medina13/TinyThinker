using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class RotateImage : MonoBehaviour
{
    public Button rotateButton;
    public float CorrectAngle = 2;
    public GameObject CorrectImage; 
    public float rotationAngle = 90f; 
    public float angleCounter = 0;
    //public AudioSource RotateCorrectlySound;
    //public AudioSource ForEveryRotateSound;
    private Theme2Level4_SceneManager sceneManagerL2_4;

    private void Start()
    {
        sceneManagerL2_4 = FindObjectOfType<Theme2Level4_SceneManager>();
        rotateButton.onClick.AddListener(Rotate);
    }

    public void Rotate()
    {
        transform.Rotate(0f, 0f, rotationAngle);
        angleCounter ++;
        Debug.Log(angleCounter);

        if (angleCounter == CorrectAngle)
        {
            //if (RotateCorrectlySound != null)
            //{
                //RotateCorrectlySound.Play();
            //}
            
            gameObject.SetActive(false);
            CorrectImage.SetActive(true);
            sceneManagerL2_4.correctPuzzle++;
            correctAnswer_Checker();
        }
        else
        {
            //ForEveryRotateSound.Play();
        }
    }

    public void correctAnswer_Checker()
    {
        if (sceneManagerL2_4.correctPuzzle == 4)
        {
            sceneManagerL2_4.IncrementFillAmount(0.16f);
            sceneManagerL2_4.correctPuzzle = 0;
            sceneManagerL2_4.fixedPuzzle++;
        }

        if (sceneManagerL2_4.fixedPuzzle == 2)
        {
            sceneManagerL2_4.confetti.SetActive(true);
            sceneManagerL2_4.nextScene_Button.gameObject.SetActive(true);
        }
    }
}
