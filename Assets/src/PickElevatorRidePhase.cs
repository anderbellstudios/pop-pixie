using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickElevatorRidePhase : APhase {
  public ElevatorEvents ElevatorEvents;
  public int DebugDefaultElevatorRide = 1;
  public List<ElevatorRide> ElevatorRides;

  public override void LocalBegin() {
    ElevatorRide ride = ElevatorRides[ElevatorRideIndex()];

    if (ride == null) {
      throw new System.Exception("ride is null");
    }

    ElevatorEvents.SetNextLevel(ride.NextLevel);

    ride.OnFinish.AddListener(PhaseFinished);
    ride.BeginRide();
  }

  private int ElevatorRideIndex() {
    int ride = ElevatorData.ElevatorRide;

    if (ride == 0) {
#if UNITY_EDITOR
      Debug.Log("Using DebugDefaultElevatorRide");
      return DebugDefaultElevatorRide;
#else
      throw new System.Exception("ElevatorRide must be set to a value greater than 0");
#endif
    }

    return ride;
  }
}
