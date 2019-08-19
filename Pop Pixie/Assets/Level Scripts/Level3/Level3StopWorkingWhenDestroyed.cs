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

  public void Updated (HitPoints hp) {
  }

  public void Decreased (HitPoints hp) {
  }

  public void BecameZero (HitPoints hp) {
    FireableScheduler.RemoveFireable( Fireable );
    DisableComponents.ForEach( comp => comp.enabled = false );
  }
}
