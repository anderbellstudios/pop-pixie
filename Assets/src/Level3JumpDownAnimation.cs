using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3JumpDownAnimation : MonoBehaviour {

  public float Duration;
  public float JumpDistance;
  public ParticleSystem ParticleSystem;

  Action Callback;
  Vector3 InitialPosition;
  IntervalTimer Timer;

  void Awake() {
    Timer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = Duration
    };
  }

  public void Perform( Action callback ) {
    Callback = callback;
    InitialPosition = transform.position;
    Timer.Reset();
  }

  void Update() {
    Timer.UnlessElapsed( UpdateJump );
    Timer.IfElapsed( FinishedJump );
  }

  void UpdateJump() {
    var position = InitialPosition + RelativePosition();
    transform.position = position;
  }

  Vector3 RelativePosition() {
    return new Vector3(
      0,
      Y(),
      0
    );
  }

  float Y() {
    float t = Timer.Progress();

    float y_factor = (3f/2f) * Mathf.Sin(
      ( Mathf.PI * t )
      + ( ( 1 - t ) * Mathf.Asin(2f/3f) )
    ) - 1f;

    return JumpDistance * y_factor;
  }

  void FinishedJump() {
    Timer.Stop();
    ParticleSystem.Play();
    Callback();
  }

}
