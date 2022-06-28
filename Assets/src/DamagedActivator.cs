using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedActivator : AActivator, IHitPointEvents {

  bool Activated = false;

  public override bool IsActivated() {
    if ( Activated ) {
      Activated = false;
      return true;
    }
    
    return false;
  }

  public void Updated (HitPoints hp) {
  }

  public void Decreased (HitPoints hp) {
    Activated = true;
  }

  public void BecameZero (HitPoints hp) {
  }

}
