using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedActivator : AActivator {
  public HitPoints OverrideHitPoints;

  bool Activated = false;

  void Awake() {
    (OverrideHitPoints ?? GetComponent<HitPoints>()).OnDecrease.AddListener(hp => {
      Activated = true;
    });
  }

  public override bool IsActivated() {
    if ( Activated ) {
      Activated = false;
      return true;
    }
    
    return false;
  }
}
