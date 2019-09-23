using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGorillaSmashAI : AEnemyAI {

  public float ChargeUpDuration;
  public ParticleSystem ParticleSystem;
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

    ParticleSystem.Play();
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
