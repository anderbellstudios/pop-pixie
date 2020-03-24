﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour {

  public RollAllowed RollAllowed;
  public MonoBehaviour DirectionManager; 
  public MovementManager MovementManager;
  public TrailRenderer TrailRenderer;

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
    if ( !WrappedInput.GetButtonDown("Roll") )
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