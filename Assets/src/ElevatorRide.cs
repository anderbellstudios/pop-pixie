using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorRide : MonoBehaviour {
  public PhaseScheduler StandardPhaseScheduler, FromShopPhaseScheduler;
  public UnityEvent OnFinish;

  public void BeginRide() {
    CheckpointData.Reset();

    PhaseScheduler scheduler = ElevatorData.ArrivedFromShop ? FromShopPhaseScheduler : StandardPhaseScheduler;

    if (scheduler == null) {
      OnFinish.Invoke();
    } else {
      scheduler.OnLastPhaseFinished.AddListener(OnFinish.Invoke);
      scheduler.BeginFirstPhase();
    }
  }
}
