using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRingPull : MonoBehaviour {

  public Rigidbody2D rb;
  public float InitialSpeed;
  public float MagnetStrength;
  public float Acceleration;
  public float Threshold;

  GameObject Magnet;
  float Speed;

  void Start() {
    rb.velocity = InitialSpeed * Random.insideUnitCircle.normalized;
    Magnet = GameObject.Find("Flying Ring Pull Magnet");
    Speed = InitialSpeed;
  }

  void Update() {
    if (StateManager.Is(State.Paused))
      return;

    Vector2 heading = Magnet.transform.position - transform.position;

    if ( heading.magnitude < Threshold ) {
      RingPullsData.Increment();
      RingPullsData.ShouldPulse = true;

      Destroy(gameObject);
    }

    Speed += Acceleration * Time.deltaTime;
    Vector2 targetVelocity = Speed * heading.normalized;
    rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, MagnetStrength);
  }

}
