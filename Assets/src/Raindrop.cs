using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raindrop : MonoBehaviour {
  public Vector2 Direction = new Vector2(-1, -1);
  public float MinSpeed, MaxSpeed;
  public float SizeRandomness;

  private Vector3 Velocity;
  private float StartY, MinStartX, MaxStartX, EndY;

  void Start() {
    if (TestMode.Enabled) {
      Destroy(gameObject);
      return;
    }

    Velocity = Direction * Random.Range(MinSpeed, MaxSpeed);

    Vector3[] parentCorners = new Vector3[4];
    ((RectTransform)transform.parent).GetWorldCorners(parentCorners);
    Vector3 bottomLeft = parentCorners[0];
    Vector3 topRight = parentCorners[2];
    float height = topRight.y - bottomLeft.y;
    float xDisplacement = height * -1 * Direction.x / Direction.y;
    StartY = topRight.y + height / 8;
    MinStartX = bottomLeft.x - Mathf.Max(0, xDisplacement);
    MaxStartX = topRight.x - Mathf.Min(0, xDisplacement);
    EndY = bottomLeft.y - height / 8;

    transform.position = new Vector3(
      Random.Range(MinStartX, MaxStartX),
      Random.Range(StartY, EndY),
      0
    );

    transform.localScale *= Random.Range(1f / SizeRandomness, SizeRandomness);
  }

  void Update() {
    if (TestMode.Enabled)
      return;

    transform.localPosition += Velocity * Time.deltaTime;

    if (transform.position.y < EndY) {
      transform.position = new Vector3(
        Random.Range(MinStartX, MaxStartX),
        StartY,
        0
      );
    }
  }
}
