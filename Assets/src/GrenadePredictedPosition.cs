using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadePredictedPosition : MonoBehaviour {
  public Transform PositionGroup;
  public RectTransform RadiusIndicator;
  public Image ExplodeDurationIndicator;
  public float AnimateInDuration;
  public float Drag = 10f;

  private float CreatedTime;

  void Awake() {
    CreatedTime = PlayingTime.time;
    transform.localScale = Vector3.zero;
  }

  void Update() {
    float scale = Mathf.Clamp(Age() / AnimateInDuration, 0f, 1f);
    transform.localScale = Vector3.one * scale;
  }

  public void Hide() {
    Destroy(gameObject);
  }

  public void UpdatePosition(
    Vector3 direction,
    float speed,
    float radius,
    float explodeTime
  ) {
    Quaternion rotation = Quaternion.FromToRotation(Vector3.right, direction);

    transform.localRotation = Quaternion.Slerp(
      transform.localRotation,
      rotation,
      0.3f
    );

    PositionGroup.rotation = Quaternion.identity;
    PositionGroup.localPosition = Vector3.right * GetMaxDistance(direction, speed);

    RadiusIndicator.localScale = Vector3.one * radius;

    // The time required for the grenade to get far enough away
    float throwDuration = DragUtils.TimeUntilDisplacement(
      speed: speed,
      drag: Drag,
      displacement: radius
    );

    float safeThrowTime = explodeTime - throwDuration;

    ExplodeDurationIndicator.fillAmount = Mathf.Clamp(Age() / safeThrowTime, 0f, 1f);
  }

  private float GetMaxDistance(Vector3 direction, float speed) {
    float maxDistance = DragUtils.MaxDisplacement(speed: speed, drag: Drag);

    RaycastHit2D hitData = Physics2D.Raycast(
      transform.position,
      direction,
      maxDistance,
      CollisionMask.ForLayer(gameObject.layer)
    );

    if (hitData.collider && hitData.distance < maxDistance) {
      return hitData.distance;
    }

    return maxDistance;
  }

  private float Age() => PlayingTime.time - CreatedTime;
}