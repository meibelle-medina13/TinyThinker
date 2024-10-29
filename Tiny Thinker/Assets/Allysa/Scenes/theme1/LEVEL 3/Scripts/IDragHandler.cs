using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //public int placed_wrong = 0;
    //public int UnplacedObjects = 3;
    public GameObject correctAnswerArea;
    private Collider2D correctAnswerCollider;
    private Vector3 originalPosition;
    //private bool isPlaced = false;
    //private bool isCorrectlyPlaced = false;
    private Theme1Level3_SceneManager scenemanagerL1_3;

    public AudioSource correctSound;
    public AudioSource wrongSound;

    void Start()
    {
        correctAnswerCollider = correctAnswerArea.GetComponent<Collider2D>();
        originalPosition = transform.position;
        scenemanagerL1_3 = FindObjectOfType<Theme1Level3_SceneManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool isCorrect = correctAnswerCollider.bounds.Contains(transform.position);
        
        if (isCorrect)
        {
            if (correctSound != null)
            {
                correctSound.Play();  
            }

            scenemanagerL1_3.IncrementPlacedCount();
            gameObject.SetActive(false);

            if (scenemanagerL1_3.placed_wrong > 0)
            {
                scenemanagerL1_3.placed_wrong--;
            }

            else
            {
                scenemanagerL1_3.IncrementFillAmount(0.16f);
            }
        }
        else
        {
            if (wrongSound != null)
            {
                wrongSound.Play();
            }

            transform.position = originalPosition;
            gameObject.SetActive(true);
            scenemanagerL1_3.placed_wrong++ ;
        }

        //if (UnplacedObjects == 0)
        //{

        //}
    }
}
