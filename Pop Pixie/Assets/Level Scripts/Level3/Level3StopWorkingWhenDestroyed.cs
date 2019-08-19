using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level3StopWorkingWhenDestroyed : MonoBehaviour, IHitPointEvents {
  public FireableScheduler FireableScheduler;
  public AFireable Fireable;
  public PolygonCollider2D Collider;
  public Oscillate Oscillate;

  public void Updated (HitPoints hp) {
  }

  public void Decreased (HitPoints hp) {
  }

  public void BecameZero (HitPoints hp) {
    FireableScheduler.RemoveFireable( Fireable );
    Collider.enabled = false;
    Oscillate.enabled = false;
  }
}
