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

  private Stopwatch Stopwatch;

  void Awake() {
    Stopwatch = new Stopwatch.PlayingTime();
    transform.localScale = Vector3.zero;
  }

  void Update() {
    transform.localScale = Vector3.one * Stopwatch.Progress(AnimateInDuration);
  }

  public void Hide() {
    Destroy(gameObject);
  }

  public void UpdatePosition(Vector3 direction, float speed, float radius, float explodeTime) {
    Quaternion rotation = Quaternion.FromToRotation(Vector3.right, direction);

    transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, 0.3f);

    PositionGroup.rotation = Quaternion.identity;
    PositionGroup.localPosition = Vector3.right * GetMaxDistance(direction, speed);

    RadiusIndicator.localScale = Vector3.one * radius;

    /**
     * The time required for the grenade to get far enough away (explosion
     * radius plus half player's height).
     */
    float throwDuration = DragUtils.TimeUntilDisplacement(
      speed: speed,
      drag: Drag,
      displacement: radius + 0.75f
    );

    float safeThrowTime = explodeTime - throwDuration;
    ExplodeDurationIndicator.fillAmount = Stopwatch.Progress(safeThrowTime);
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
}
