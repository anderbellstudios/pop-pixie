using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayerAI : AMovementEnemyAI {
  [field: SerializeField]
  public override float Speed { get; set; }

  protected override void WhileActive() {
    Helper.MoveTowardsPlayer(Speed);
  }
}
