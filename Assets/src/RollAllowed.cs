using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAllowed : MonoBehaviour {
  public float Cooldown;

  private Stopwatch Stopwatch = null;

  public bool CanRoll() {
    if (Stopwatch == null)
      return true;
    return Stopwatch.Time() > Cooldown;
  }

  public void DidRoll() {
    Stopwatch = new Stopwatch.PlayingTime();
  }
}
