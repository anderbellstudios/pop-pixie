using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigateToPlayerAI : AMovementEnemyAI {
  public float Speed;

  protected override void OnActivate() {
    Helper.EnableNavigateToPoint(Speed);
  }

  protected override void OnDeactivate() {
    Helper.DisableNavigateToPoint();
  }

  protected override void WhileActive() {
    Helper.NavigateToPlayer();
  }
}
