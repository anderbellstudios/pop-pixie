using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinAI : AMovementEnemyAI {
  public bool Alerted = false;
  public WaitForAlertAI WaitForAlertAI;
  public AMovementEnemyAI UnalertedAI, AlertedAI;

  void Start() {
    Activate();

    WaitForAlertAI.OnAlert.AddListener(() => {
      Alerted = true;
    });
  }

  protected override void UseChildAIs(Action<AGenericEnemyAI> useChild) {
    if (!Alerted) {
      useChild(WaitForAlertAI);
    }
  }

  protected override AMovementEnemyAI UseMovementAI()
    => Alerted ? AlertedAI : UnalertedAI;
}
