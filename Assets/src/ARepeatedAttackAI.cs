using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ARepeatedAttackAI : AMovementEnemyAI {
  public float AttackInterval;
  public int MinAttacks,
    MaxAttacks;

  private float CurrentAttackInterval;

  protected override void OnActivate() {
    // Ignore changes to AttackInterval while attack is in progress
    CurrentAttackInterval = AttackInterval;
    int attacks = Random.Range(MinAttacks, MaxAttacks);
    PerformAttackAndScheduleNext(attacks);
    LocalOnActivate();
  }

  protected virtual void LocalOnActivate() { }

  private void PerformAttackAndScheduleNext(int remainingAttacks) {
    if (remainingAttacks == 0) {
      OnFinish();
      return;
    }

    PerformAttack();

    Helper.SetTimeout(
      () => {
        PerformAttackAndScheduleNext(remainingAttacks - 1);
      },
      CurrentAttackInterval
    );
  }

  protected abstract void PerformAttack();
}
