using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate float SpeedModifier(float s);

public class MovementManager : MonoBehaviour {
  public List<SpeedModifier> SpeedModifiers = new List<SpeedModifier>();
  public Animator Animator;
  public Rigidbody2D rb;
  public UnityEvent OnFootstep;

  public Vector2 _Movement, VisualMovement;
  public Vector2 Movement {
    get { return _Movement; }

    set {
      Vector2 diff = value - _Movement;
      _Movement = value;
      VisualMovement += diff;
    }
  }

  void Update() {
    if (Animator != null) {
      Animator.SetInteger("Movement Direction", VisualMovement.x > 0 ? 1 : -1);
      Animator.SetBool("Walking", StatePermitsMovement() && VisualMovement.magnitude > 0);
      Animator.SetFloat("Speed", Velocity(VisualMovement).magnitude / Time.deltaTime);
    }

    VisualMovement = Vector2.zero;
  }

  void FixedUpdate() {
    if (StatePermitsMovement())
      rb.MovePosition(rb.position + Velocity(Movement));

    _Movement = Vector2.zero;
  }

  public void DispatchFootstepDown() {
    OnFootstep.Invoke();
  }

  bool StatePermitsMovement() {
    return StateManager.Enabled(StateFeatures.Movement);
  }

  Vector2 Velocity(Vector2 movement) {
    return ModifiedSpeed() * movement;
  }

  float ModifiedSpeed() {
    float speed = 1f;

    foreach (var modifier in SpeedModifiers) {
      speed = modifier(speed);
    }

    return speed;
  }
}
