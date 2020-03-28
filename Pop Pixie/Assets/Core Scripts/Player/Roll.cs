using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour {

  public RollAllowed RollAllowed;
  public MonoBehaviour DirectionManager; 
  public MovementManager MovementManager;
  public TrailRenderer TrailRenderer;
  public Animator Animator;

  public float Speed;
  public float Duration;

  public bool Rolling;

  void Update() {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    InitateOpportunity();
  }

  void FixedUpdate() {
    TrailRenderer.emitting = Rolling;

    if ( StateManager.Isnt( State.Playing ) )
      return;

    if ( Rolling ) {
      MovementManager.Movement += Speed * Direction();
    }

  }

  void InitateOpportunity() {
    if ( WrappedInput.GetButtonDown("Roll") && !Rolling && RollAllowed.CanRoll() )
      StartRolling(); 
  }

  void StartRolling() {
    RollAllowed.DidRoll();
    Rolling = true;
    Invoke("StopRolling", Duration);
    Animator.SetTrigger("Roll");
  }

  void StopRolling() {
    Rolling = false;
  }

  Vector2 Direction() {
    return ( (IDirectionManager) DirectionManager ).Direction.normalized;
  }

}
