using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float SpeedModifier(float s);

public class MovementManager : MonoBehaviour {

  public List<SpeedModifier> SpeedModifiers = new List<SpeedModifier>();
  public Animator Animator;

  public Rigidbody2D rb;
  public Vector2 Movement;

  void FixedUpdate() {
    Vector2 velocity = ModifiedSpeed() * Movement;

    bool playingState = StateManager.Is( State.Playing );

    if ( playingState ) 
      rb.MovePosition(rb.position + velocity);

    if ( Animator != null ) {
      Animator.SetInteger("Movement Direction", velocity.x > 0 ? 1 : -1);
      Animator.SetBool("Walking", playingState && velocity.magnitude >= 0.5f);
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
