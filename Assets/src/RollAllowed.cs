using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAllowed : MonoBehaviour {
  public Crouch Crouch;
  public float Cooldown;

  private Stopwatch Stopwatch = null;

  public bool CanRoll() => !Crouch.Crouching && (
    Stopwatch == null || Stopwatch.Time() > Cooldown
  );

  public void DidRoll() {
    Stopwatch = new Stopwatch.PlayingTime();
  }
}
