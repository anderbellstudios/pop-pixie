using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGorillaSmashAI : AEnemyAI {

  public float RadiusExpandDuration;
  public Transform RadiusIndicator;
  public float ChargeUpDuration;
  public ParticleSystem ParticleSystem;
  public float DamageRadius;
  public float Damage;
  public GameObject Explosion;
  public AEnemyAI AfterAtack;

  IntervalTimer RadiusExpandTimer;
  IntervalTimer AttackTimer;

  public override void ControlGained() {
    RadiusExpandTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = RadiusExpandDuration
    };

    RadiusExpandTimer.Reset();

    AttackTimer = new IntervalTimer() {
      Interval = ChargeUpDuration
    };

    AttackTimer.Reset();

    ParticleSystem.Play();
  }

  public override void WhileInControl() {
    SetRadiusIndicatorScale( RadiusExpandTimer.Progress() );
    AttackTimer.IfElapsed( DoAttack );
  }

  void DoAttack() {
    Instantiate( Explosion, transform );

    if ( TargetDistance() <= DamageRadius )
      DamageTarget( Damage );

    RelinquishControlTo( AfterAtack );
  }

  public override void ControlRelinquished() {
    SetRadiusIndicatorScale(0f);
  }

  void SetRadiusIndicatorScale( float scale ) {
    RadiusIndicator.localScale = new Vector3( scale, scale, scale );
  }

}
