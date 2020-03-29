using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float SpeedModifier(float s);

public class MovementManager : MonoBehaviour {

  public List<SpeedModifier> SpeedModifiers;
  public Animator Animator;

  public Rigidbody2D rb;
  public Vector2 Movement;

  void Awake() {
    SpeedModifiers = new List<SpeedModifier>();
  }

  void FixedUpdate() {
    if ( StateManager.Is( State.Playing ) ) {
      Vector2 velocity = ModifiedSpeed() * Movement;

      if ( Animator != null ) {
        int facing = velocity.x >= 0 ? 1 : -1;
        Animator.SetInteger("Direction", facing);

        float speed = velocity.magnitude;
        Animator?.SetBool("Walking", speed >= 0.5f);
      }

      rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
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
