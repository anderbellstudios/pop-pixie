using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ARepeatedAttackAI : AEnemyAI {
  public float AttackInterval;
  public int MinAttacks, MaxAttacks;

  public AEnemyAI WhenFinished;

  IntervalTimer AttackTimer;
  int RemainingAttacks;

  public override void ControlGained() {
    AttackTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = AttackInterval
    };

    AttackTimer.Start();

    RemainingAttacks = Random.Range(MinAttacks, MaxAttacks);

    LocalControlGained();
  }

  public virtual void LocalControlGained() {}

  public override void WhileInControl() {
    AttackTimer.IfElapsed(PerformAttackAndDecrementCounter);
    LocalWhileInControl();
  }

  public virtual void LocalWhileInControl() {}

  void PerformAttackAndDecrementCounter() {
    if (RemainingAttacks > 0)
    {
        PerformAttack();
        RemainingAttacks--;
    }

    if (RemainingAttacks == 0)
    {
        EndAttack();
    }
  }

  protected void EndAttack() {
    RelinquishControlTo(WhenFinished);
  }

  public abstract void PerformAttack();
}
