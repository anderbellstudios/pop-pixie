using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level3StopWorkingWhenDestroyed : MonoBehaviour, IHitPointEvents {
  public FireableScheduler FireableScheduler;
  public AFireable Fireable;

  bool StoppedWorking = false;

  public void Updated (HitPoints hp) {
    if ( !StoppedWorking && hp.Current == 0 )
      StopWorking();
  }

  public void Decreased (HitPoints hp) {
  }

  public void BecameZero (HitPoints hp) {
  }

  void StopWorking() {
    StoppedWorking = true;
    FireableScheduler.RemoveFireable( Fireable );
  }
}
