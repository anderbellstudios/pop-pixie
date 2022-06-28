using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAllowed : MonoBehaviour {

  public float Cooldown;
  private DateTime LastRolled;

  public bool CanRoll() {
    return SecondsSinceRoll() >= Cooldown;
  }

  public void DidRoll() {
    LastRolled = DateTime.Now;
  }

  double SecondsSinceRoll() {
    return DateTime.Now.Subtract( LastRolled ).TotalSeconds;
  }

}
