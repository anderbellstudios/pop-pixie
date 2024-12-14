using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologremAI : AMovementEnemyAI {
  [field: SerializeField]
  public override float Speed { get; set; }

  void Start() {
    Activate();
    Helper.OnPlayerCollision(PerformAttack);
  }

  protected override void WhileActive() {
    if (Helper.CanMoveToPlayer()) {
      Helper.MoveTowardsPlayer(Speed);
    }
  }

  private void PerformAttack() {
    bool isCounterAttack = Helper.DamagePlayer(1, true);

    if (isCounterAttack) {
      DamageHitPointsInRadius.Invoke(
        damage: 1,
        origin: Helper.Position,
        radius: 5
      );
    } else {
      Destroy(Helper.GameObject);
    }
  }
}
