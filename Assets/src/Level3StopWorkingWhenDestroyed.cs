using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level3StopWorkingWhenDestroyed : MonoBehaviour {
  public HitPoints OverrideHitPoints;
  public FireableScheduler FireableScheduler;
  public AFireable Fireable;

  bool StoppedWorking = false;

  void Awake() {
    (OverrideHitPoints ?? GetComponent<HitPoints>()).OnUpdate.AddListener(hp => {
      if (!StoppedWorking && hp.Current == 0) {
        StoppedWorking = true;
        FireableScheduler.RemoveFireable(Fireable);
      }
    });
  }
}
