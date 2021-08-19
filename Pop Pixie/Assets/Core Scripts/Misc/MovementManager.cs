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

  void Update() {
    if ( Animator != null ) {
      Animator.SetInteger("Movement Direction", Movement.x > 0 ? 1 : -1);
      Animator.SetBool("Walking", StateManager.Is(State.Playing) && Movement.magnitude > 0);
      Animator.SetFloat("Speed", Velocity().magnitude / Time.deltaTime);
    }
  }

  void FixedUpdate() {
    if (StateManager.Is(State.Playing)) 
      rb.MovePosition(rb.position + Velocity());

    Movement = Vector2.zero;
  }

  Vector2 Velocity() {
    return ModifiedSpeed() * Movement;
  }

  float ModifiedSpeed() {
    float speed = 1f;

    foreach (var modifier in SpeedModifiers) {
      speed = modifier(speed);
    }

    return speed;
  }

}
