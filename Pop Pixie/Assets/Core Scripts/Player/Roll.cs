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

  private ButtonPressHelper ButtonPressHelper = new SingleButtonPressHelper();

  void Start() {
    StateChanged();
  }

  void Update() {
    if (!StateManager.Playing) {
      ButtonPressHelper.Clear();
      return;
    }

    if (ButtonPressHelper.GetButtonPress("Roll") && !Rolling && RollAllowed.CanRoll())
      StartRolling(); 

    if (Rolling) {
      MovementManager.Movement += Speed * Direction() * Time.deltaTime;
    }
  }

  void StartRolling() {
    RollAllowed.DidRoll();
    Rolling = true;
    Invoke("StopRolling", Duration);
    Animator.SetTrigger("Roll");
    StateChanged();
  }

  void StopRolling() {
    Rolling = false;
    StateChanged();
  }

  void StateChanged() {
    TrailRenderer.emitting = Rolling;
    gameObject.layer = LayerMask.NameToLayer(Rolling ? "PlayerRolling" : "Player");
  }

  Vector2 Direction() {
    return ( (IDirectionManager) DirectionManager ).Direction.normalized;
  }

}
