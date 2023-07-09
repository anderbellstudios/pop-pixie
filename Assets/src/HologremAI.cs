using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologremAI : AEnemyAI, IRequiresLineOfMovementAI {

  public float Speed;

  public override void WhileInControl() {
    if (LineOfMovement())
      ApplyMovement(TargetDirection() * Speed);
  }

  public override void LocalOnCollisionEnter2D(Collision2D col) {
    if (col.gameObject == Target)
      PerformAttack();
  }

  void PerformAttack() {
    bool isCounterAttack = Target.GetComponent<HitPoints>().Damage(1, true);

    if (isCounterAttack) {
      DamageHitPointsInRadius.Invoke(
        1, // Damage
        transform.position,
        5 // Radius
      );
    } else { 
      Destroy(gameObject);
    }
  }
}
