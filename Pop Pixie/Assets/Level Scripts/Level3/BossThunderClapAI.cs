using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThunderClapAI : AEnemyAI {

  public float ChargeUpDuration;
  public float StopAimingThreshold;
  public ParticleSystem ParticleSystem;
  public float FireDuration;
  public float LineWidth;
  public float Damage;
  public LineRenderer LineRenderer;
  public AEnemyAI AfterAtack;

  Vector3 Direction;

  IntervalTimer ChargingUpTimer;
  IntervalTimer FiringTimer;

  public override void ControlGained() {
    ChargingUpTimer = new IntervalTimer() {
      Interval = ChargeUpDuration
    };

    FiringTimer = new IntervalTimer() {
      Interval = FireDuration
    };

    ChargingUpTimer.Reset();

    ParticleSystem.Play();
  }

  public override void WhileInControl() {
    if ( ChargingUpTimer.Started && ChargingUpTimer.Progress() < StopAimingThreshold )
      Direction = TargetDirection();

    ChargingUpTimer.IfElapsed( FireLaser );
    FiringTimer.IfElapsed( StopFiring );

    LineRenderer.enabled = FiringTimer.Started && !FiringTimer.Elapsed();

    float w = LineWidth * ( 1 - FiringTimer.Progress() );
    LineRenderer.SetWidth(w, w);
  }

  void FireLaser() {
    LineRenderer.SetPosition( 0, transform.position );
    LineRenderer.SetPosition( 1, 1000000 * Direction );

    ChargingUpTimer.Stop();
    FiringTimer.Reset();

    var hit = Physics2D.CircleCast( 
      transform.position, 
      LineWidth / 2, 
      Direction,
      Mathf.Infinity,
      1 << 12 // <-- only collide with player
    );

    if ( hit.collider != null )
      DamageTarget( Damage );
  }

  void StopFiring() {
    FiringTimer.Stop();
    RelinquishControlTo( AfterAtack );
  }

}
