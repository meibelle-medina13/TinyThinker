using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
  public RectTransform platform;
  public RectTransform start;
  public RectTransform end;
  public float speed;
  int direction = 1;

  private void Update()
  {
    Vector2 destination = TargetDestination();

    platform.transform.localPosition = Vector2.Lerp(platform.transform.localPosition, destination, speed * Time.deltaTime);

    float distance = (destination - (Vector2)platform.transform.localPosition).magnitude;

    if (distance <= 100)
    {
      direction *= -1;
    }
  }

  private void OnDrawGizmos()
  {
    if (platform != null && start != null && end != null)
    {
      Gizmos.DrawLine(platform.transform.position, start.position);
      Gizmos.DrawLine(platform.transform.position, end.position);
    }
  }

  private Vector2 TargetDestination()
  {
    if (direction == 1)
    {
      return start.transform.localPosition;
    }
    else
    {
      return end.transform.localPosition;
    }
  }
}
