using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(LineRenderer))]

public class MatchingType : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private bool isDragging, isMatched, isDone;
    private int totalItems;
    private Vector3 destination;

    [SerializeField]
    private int score;

    [SerializeField]
    private int error;

    [SerializeField]
    private GameObject confetti;

    [SerializeField]
    private AudioSource SFX;
    [SerializeField]
    private AudioClip wrong;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        totalItems = 100 / score;
        PlayerPrefs.SetInt("MatchingType Items", totalItems);
        Debug.Log(totalItems);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                lineRenderer.SetPosition(0, mousePosition);
            }
        }

        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            lineRenderer.SetPosition(1, mousePosition);
            destination = mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            RaycastHit2D hit = Physics2D.Raycast(destination, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.name == gameObject.name)
            {
                if (!isMatched)
                {
                    Debug.Log(gameObject.name);
                    Debug.Log(score);
                    isMatched = true;
                    PlayerPrefs.SetInt("MatchingType Score", score);
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    int items = PlayerPrefs.GetInt("MatchingType Items");
                    PlayerPrefs.SetInt("MatchingType Items", items - 1);
                }
            }
            else if (hit.collider != null && hit.collider.gameObject.name != gameObject.name)
            {
                lineRenderer.positionCount = 0;
                score -= error;
                if (score < 0)
                {
                    score = 0;
                }
                SFX.clip = wrong;
                SFX.Play();
                Debug.Log(gameObject.name);
            }
            else
            {
                lineRenderer.positionCount = 0;
            }
            
            lineRenderer.positionCount = 2;

            Debug.Log("total items" +PlayerPrefs.GetInt("MatchingType Items"));
            totalItems = PlayerPrefs.GetInt("MatchingType Items");
            if (totalItems == 0)
            {
                confetti.SetActive(true);
            }
        }
    }

}
