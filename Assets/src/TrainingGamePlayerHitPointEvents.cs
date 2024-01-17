using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingGamePlayerHitPointEvents : MonoBehaviour {
  public HitPoints OverrideHitPoints;

  void Awake() {
    (OverrideHitPoints ?? GetComponent<HitPoints>()).OnDecrease.AddListener(hp => {
      SimulationResultData.NumberOfHitsTaken++;
      DamagedBlur.Current.Activate();
    });
  }
}
