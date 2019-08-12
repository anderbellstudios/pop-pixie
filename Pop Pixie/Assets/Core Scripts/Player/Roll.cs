using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour {

  public Rigidbody2D rb;
  public RollAllowed RollAllowed;
  public MonoBehaviour DirectionManager; 
  public PlayerMovable PlayerMovable;

  public float Speed;
  public float Duration;

  public bool Rolling;

  void FixedUpdate() {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    InitateOpportunity();

    if ( Rolling ) {
      PlayerMovable.Movement += Speed * Direction();
    }

  }

  void InitateOpportunity() {
    if ( !Input.GetButton("Roll") )
      return; // Since the player isn't trying to roll

    if ( !Rolling && RollAllowed.CanRoll() ) {
      RollAllowed.DidRoll();
      Rolling = true;
      Invoke("StopRolling", Duration);
    }

  }

  void StopRolling() {
    Rolling = false;
  }

  Vector2 Direction() {
    return ( (IDirectionManager) DirectionManager ).Direction.normalized;
  }

}
