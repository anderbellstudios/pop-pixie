using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Level3StopWorkingWhenDestroyed : MonoBehaviour, IHitPointEvents {
  public FireableScheduler FireableScheduler;
  public AFireable Fireable;
  public List<Behaviour> DisableComponents;

  public bool StoppedWorking = false;

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
    DisableComponents.ForEach( comp => comp.enabled = false );
  }
}
