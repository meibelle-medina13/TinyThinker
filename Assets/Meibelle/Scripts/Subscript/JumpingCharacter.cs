using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JumpingCharacter : MonoBehaviour
{
  Rigidbody2D characterRB;
  GameObject parent;
  private float jumpForce;
  private float moveVertical;
  private bool isJumping;

  private void Start()
  {
    characterRB = GetComponent<Rigidbody2D>();
    parent = gameObject.transform.parent.gameObject;
    jumpForce = 200f;
    isJumping = false;
  }

  private void Update()
  {
    if (isJumping)
    {
      gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
    }

    if (gameObject.transform.localPosition.y <= -300)
    {
      gameObject.transform.SetParent(parent.transform);
      gameObject.transform.localPosition = new Vector3(6, -211, 0);
      PlayerPrefs.SetString("Falling", "True");
    }
  }

  private void FixedUpdate()
  {
    if (!isJumping && moveVertical > 0.1f)
    {
      characterRB.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Platform"))
    {
      isJumping = false;
      gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
    }
    else if (collision.gameObject.CompareTag("Cloud Platform"))
    {
      gameObject.transform.SetParent(collision.gameObject.transform);
      
      if (gameObject.transform.localPosition.y >= 90)
      {
        isJumping = false;
        gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
        if (collision.gameObject.name == "jump cloud - finish line")
        {
          gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
          PlayerPrefs.SetString("Collider", "final cloud");
        }
      }
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    string[] tags = { "Platform", "Cloud Platform" };

    if (tags.Contains(collision.gameObject.tag))
    {
      isJumping = true;
    }
  }

  public void MoveCharacter(int status)
  {
    moveVertical = status;
  }
}
