using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickElevatorRidePhase : APhase {
  public ElevatorEvents ElevatorEvents;
  public int DebugDefaultElevatorRide = 1;
  public List<ElevatorRide> ElevatorRides;

  void Awake() {
    if (LevelCompletionData.LevelCompleted == 0) {
#if UNITY_EDITOR
      Debug.Log("Using DebugDefaultElevatorRide");
      LevelCompletionData.CompleteLevel(DebugDefaultElevatorRide);
#else
      throw new System.Exception("LevelCompleted must be greater than 0");
#endif
    }
  }

  public override void LocalBegin() {
    ElevatorRide ride = ElevatorRides[LevelCompletionData.LevelCompleted];

    if (ride == null) {
      throw new System.Exception("ride is null");
    }

    if (LevelCompletionData.PlayElevatorRide) {
      ride.OnFinish.AddListener(PhaseFinished);
      ride.BeginRide();
    } else {
      PhaseFinished();
    }
  }
}
