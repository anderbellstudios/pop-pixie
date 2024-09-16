using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ARepeatedAttackAI : AEnemyAI {
  public float AttackInterval;
  public int MinAttacks, MaxAttacks;

  public AEnemyAI WhenFinished;

  private float CurrentAttackInterval;

  public override void ControlGained() {
    // Ignore changes to AttackInterval while attack is in progress
    CurrentAttackInterval = AttackInterval;
    int attacks = Random.Range(MinAttacks, MaxAttacks);
    PerformAttackAndScheduleNext(attacks);
    LocalControlGained();
  }

  public virtual void LocalControlGained() { }

  private void PerformAttackAndScheduleNext(int remainingAttacks) {
    if (remainingAttacks == 0) {
      EndAttack();
      return;
    }

    PerformAttack();

    SetTimeout(() => {
      PerformAttackAndScheduleNext(remainingAttacks - 1);
    }, CurrentAttackInterval);
  }

  protected void EndAttack() {
    RelinquishControlTo(WhenFinished);
  }

  public abstract void PerformAttack();
}
