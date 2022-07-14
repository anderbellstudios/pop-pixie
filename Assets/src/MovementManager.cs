using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float SpeedModifier(float s);

public class MovementManager : MonoBehaviour {

  public List<SpeedModifier> SpeedModifiers = new List<SpeedModifier>();

  public Animator Animator;

  public SoundController SoundController;
  public List<AudioClip> FootstepSounds;
  private int currentFootstepSound = 0;

  public Rigidbody2D rb;

  public Vector2 _Movement, VisualMovement;
  public Vector2 Movement {
    get { return _Movement; }

    set {
      _Movement = value;
      VisualMovement = value;
    }
  }

  void Update() {
    if ( Animator != null ) {
      Animator.SetInteger("Movement Direction", VisualMovement.x > 0 ? 1 : -1);
      Animator.SetBool("Walking", StatePermitsMovement() && VisualMovement.magnitude > 0);
      Animator.SetFloat("Speed", Velocity(VisualMovement).magnitude / Time.deltaTime);
    }

    VisualMovement = Vector2.zero;
  }

  void FixedUpdate() {
    if (StatePermitsMovement())
      rb.MovePosition(rb.position + Velocity(Movement));

    Movement = Vector2.zero;
  }

  public void OnFootstepDown() {
    SoundController.Play(FootstepSounds[currentFootstepSound]);
    currentFootstepSound = (currentFootstepSound + 1) % FootstepSounds.Count;
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
