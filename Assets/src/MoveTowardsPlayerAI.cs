using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayerAI : AMovementEnemyAI {
  public float Speed;

  protected override void WhileActive() {
    Helper.MoveTowardsPlayer(Speed);
  }
}
