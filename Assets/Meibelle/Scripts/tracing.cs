using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tracing : MonoBehaviour
{
    public GameObject Scene4;
    public GameObject Pencil;
    public GameObject PencilMask;
    public GameObject Collider;
    private Vector3 pencilState;
    private Vector3 pencilRaise = new Vector3(105, 120, 0);
    private Vector3 pencilWrite = new Vector3(85, 100, 0);

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane + 1;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (Input.GetMouseButton(0))
        {
            Collider.GetComponent<CircleCollider2D>().enabled = true;
            pencilState = pencilWrite;

            GameObject pencilMask = Instantiate(PencilMask, worldPosition, Quaternion.identity);
            pencilMask.transform.SetParent(Scene4.transform);
        }
        else
        {
            Collider.GetComponent<CircleCollider2D>().enabled = false;
            pencilState = pencilRaise;
        }

        Pencil.transform.position = worldPosition + pencilState;
        Collider.transform.position = worldPosition;
    }

    HashSet<string> tracedPoints = new HashSet<string>();

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.name);
        if (collider.CompareTag("Tracing Point") && !tracedPoints.Contains(collider.gameObject.name))
        {
            tracedPoints.Add(collider.gameObject.name);
            Debug.Log(collider.gameObject.name);
        }
    }           
}
