using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAllowed : MonoBehaviour {
  public float Cooldown;

  private float LastRolled = -1000000;

  public bool CanRoll() {
    return (PlayingTime.time - LastRolled) > Cooldown;
  }

  public void DidRoll() {
    LastRolled = PlayingTime.time;
  }
}
