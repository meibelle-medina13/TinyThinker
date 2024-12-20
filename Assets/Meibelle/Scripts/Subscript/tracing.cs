using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tracing : MonoBehaviour
{
    public GameObject TracingPanel;
    public GameObject Pencil;
    public GameObject PencilMask;
    public GameObject Collider;
    private Vector3 pencilState;
    private Vector3 pencilRaise = new Vector3(105, 120, 0);
    private Vector3 pencilWrite = new Vector3(85, 100, 0);


    void Update()
    {
        if (TracingPanel.activeSelf == true)
        {
            Vector3 screenPosition = Input.mousePosition;
            screenPosition.z = Camera.main.nearClipPlane + 1;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            if (Input.GetMouseButton(0))
            {
                Collider.GetComponent<CircleCollider2D>().enabled = true;
                pencilState = pencilWrite;

                GameObject pencilMask = Instantiate(PencilMask, worldPosition, Quaternion.identity);
                pencilMask.transform.SetParent(TracingPanel.transform);
            }
            else
            {
                Collider.GetComponent<CircleCollider2D>().enabled = false;
                pencilState = pencilRaise;
            }

            Pencil.transform.position = worldPosition + pencilState;
            Collider.transform.position = worldPosition;
        }
    }

    HashSet<string> tracedPoints = new HashSet<string>();
    public int no_of_traced = 0;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Tracing Point") && !tracedPoints.Contains(collider.gameObject.name))
        {
            tracedPoints.Add(collider.gameObject.name);
            no_of_traced++;
            PlayerPrefs.SetInt("Tracing Points", no_of_traced);
            Debug.Log(no_of_traced);
        }
    }           
}
