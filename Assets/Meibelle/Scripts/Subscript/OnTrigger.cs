using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class OnTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() == null)
        {
            collision.gameObject.SetActive(false);
            PlayerPrefs.SetString("Collider", collision.gameObject.name);
        }
    }
}
