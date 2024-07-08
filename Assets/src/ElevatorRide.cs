using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorRide : MonoBehaviour {
  public string NextLevel;

  public PhaseScheduler StandardPhaseScheduler, FromShopPhaseScheduler;
  public UnityEvent OnFinish;

  void Awake() {
    if (NextLevel == "") {
      throw new System.Exception("NextLevel cannot be empty");
    }
  }

  public void BeginRide() {
    PhaseScheduler scheduler = ElevatorData.ArrivedFromShop ? FromShopPhaseScheduler : StandardPhaseScheduler;

    if (scheduler == null) {
      OnFinish.Invoke();
    } else {
      scheduler.OnLastPhaseFinished.AddListener(OnFinish.Invoke);
      scheduler.BeginFirstPhase();
    }
  }
}
