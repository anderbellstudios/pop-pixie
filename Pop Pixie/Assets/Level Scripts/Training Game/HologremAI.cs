using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologremAI : AEnemyAI, IRequiresLineOfMovementAI {

  public float Speed;
  public DamagedBlur DamagedBlur;

  public override void WhileInControl() {
    if (LineOfMovement())
      ApplyMovement(TargetDirection() * Speed);
  }

  public override void LocalOnCollisionEnter2D(Collision2D col) {
    if (col.gameObject == Target)
      PerformAttack();
  }

  void PerformAttack() {
    DamagedBlur.Activate();
    GetComponent<HitPoints>().Damage(1);
  }

}