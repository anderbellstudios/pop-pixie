using System;
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
    Vector2 velocity = ModifiedSpeed() * Movement;

    bool playingState = StateManager.Is( State.Playing );

    if ( playingState ) 
      rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

    if ( Animator != null ) {
      int direction = (int) Math.Round( Mathf.Clamp( velocity.x, -1, 1 ) );
      Animator.SetInteger("Movement Direction", direction);

      float speed = velocity.magnitude;
      Animator.SetBool("Walking", playingState && speed >= 0.5f);
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
