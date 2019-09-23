using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWaitingToAttackAI : AEnemyAI {

  public float Speed;
  public float AttackInterval;
  public float AttackIntervalRandomness;

  public AEnemyAI WhenAttacking;

  IntervalTimer AttackTimer;

  public override void ControlGained() {
    float attackIntervalRandomModifier = Random.Range(
      - AttackIntervalRandomness / 2,
        AttackIntervalRandomness / 2
    );

    AttackTimer = new IntervalTimer() {
      Interval = AttackInterval + attackIntervalRandomModifier
    };

    AttackTimer.Reset();
  }

  public override void WhileInControl() {
    ApplyMovement( Speed * TargetDirection() );

    AttackTimer.IfElapsed(
      () => RelinquishControlTo( WhenAttacking )
    );
  }

}
