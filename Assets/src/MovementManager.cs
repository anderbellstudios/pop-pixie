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
  public Vector2 ConveyorContactOffset = Vector2.zero;
  public UnityEvent OnFootstep;

  public Vector2 Movement { get; private set; }

  private Vector2 VisualMovement;

  public void Move(
    Vector2 amount,
    bool skipVisualMovement = false,
    bool skipSpeedModifiers = false
  ) {
    Vector2 modifiedAmount = amount * (skipSpeedModifiers ? 1f : ModifiedSpeed());

    Movement += modifiedAmount;

    if (!skipVisualMovement) {
      VisualMovement += modifiedAmount;
    }
  }

  public Vector2 ConveyorContactPoint => rb.position + ConveyorContactOffset;

  void Update() {
    if (Animator != null) {
      Animator.SetInteger("Movement Direction", VisualMovement.x > 0 ? 1 : -1);
      Animator.SetBool("Walking", StatePermitsMovement() && VisualMovement.magnitude > 0);
      Animator.SetFloat("Speed", VisualMovement.magnitude / Time.deltaTime);
    }

    VisualMovement = Vector2.zero;
  }

  void FixedUpdate() {
    if (StatePermitsMovement())
      rb.MovePosition(rb.position + Movement);

    Movement = Vector2.zero;
  }

  public void DispatchFootstepDown() {
    OnFootstep.Invoke();
  }

  bool StatePermitsMovement() {
    return StateManager.Enabled(StateFeatures.Movement);
  }

  float ModifiedSpeed() {
    float speed = 1f;

    foreach (var modifier in SpeedModifiers) {
      speed = modifier(speed);
    }

    return speed;
  }
}
