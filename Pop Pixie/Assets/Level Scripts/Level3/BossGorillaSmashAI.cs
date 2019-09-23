using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGorillaSmashAI : AEnemyAI {

  public float ChargeUpDuration;
  public float DamageRadius;
  public float Damage;
  public GameObject Explosion;
  public AEnemyAI AfterAtack;

  IntervalTimer AttackTimer;

  public override void ControlGained() {
    AttackTimer = new IntervalTimer() {
      Interval = ChargeUpDuration
    };

    AttackTimer.Reset();
  }

  public override void WhileInControl() {
    AttackTimer.IfElapsed( DoAttack );
  }

  void DoAttack() {
    Instantiate( Explosion, transform );

    if ( TargetDistance() <= DamageRadius )
      DamageTarget( Damage );

    RelinquishControlTo( AfterAtack );
  }

}
