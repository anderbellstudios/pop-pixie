using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRingPull : MonoBehaviour {

  public Rigidbody2D rb;
  public float InitialSpeed;
  public float Acceleration;
  public float Threshold;

  GameObject Magnet;
  float CameraScale;

  void Start() {
    CameraScale = ComputeCameraScale();
    rb.velocity = CameraScale * InitialSpeed * Random.insideUnitCircle.normalized;
    Magnet = GameObject.Find("Flying Ring Pull Magnet");
  }

  void Update() {
    if (!StateManager.Enabled(StateFeatures.BackgroundAnimations))
      return;

    Vector2 heading = Magnet.transform.position - transform.position;

    if (heading.magnitude < CameraScale * Threshold) {
      RingPullsData.Increment();
      RingPullsData.ShouldPulse = true;

      Destroy(gameObject);
    }

    rb.velocity += CameraScale * Acceleration * (1f / Mathf.Sqrt(heading.magnitude)) * Time.deltaTime * heading.normalized;
  }

  float ComputeCameraScale() {
    Vector2 a = Camera.main.WorldToScreenPoint(new Vector2(0, 0));
    Vector2 b = Camera.main.WorldToScreenPoint(new Vector2(1, 0));

    return (b - a).magnitude;
  }

}
