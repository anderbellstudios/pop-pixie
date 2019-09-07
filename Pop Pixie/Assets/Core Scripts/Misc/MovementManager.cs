using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float SpeedModifier(float s);

public class MovementManager : MonoBehaviour {

  public List<SpeedModifier> SpeedModifiers;

  public Rigidbody2D rb;
  public Vector2 Movement;

  void Awake() {
    SpeedModifiers = new List<SpeedModifier>();
  }

  void FixedUpdate() {
    if ( StateManager.Is( State.Playing ) ) {
      rb.MovePosition(rb.position + ModifiedSpeed() * Movement * Time.fixedDeltaTime);
    }

    Movement = Vector2.zero;
  }

  float ModifiedSpeed() {
    float speed = 1f;

    foreach (var modifier in SpeedModifiers) {
      speed = modifier(speed);
    }

    return speed;
  }

}
