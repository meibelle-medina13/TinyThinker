using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class OnTrigger : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.DeleteKey("Collider");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.name == "star" || gameObject.name == "final star")
        {
            gameObject.SetActive(false);
            PlayerPrefs.SetString("Collider", gameObject.name);
        }
        else if (collision.gameObject.GetComponent<Rigidbody2D>() == null)
        {
            collision.gameObject.SetActive(false);
            PlayerPrefs.SetString("Collider", collision.gameObject.name);
            PlayerPrefs.SetString("Trigger", gameObject.name);
        }
        else
        {
            PlayerPrefs.SetString("Collider", collision.gameObject.name);
            Debug.Log("COLLIDEEE");
        }
    }
}
