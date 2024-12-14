using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigateToPlayerAI : AMovementEnemyAI {
  [field: SerializeField]
  public override float Speed { get; set; }

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
