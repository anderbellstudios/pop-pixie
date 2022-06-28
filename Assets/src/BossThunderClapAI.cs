using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThunderClapAI : AEnemyAI {

  public MonoBehaviour DirectionManager;
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
      TimeClass = "PlayingTime",
      Interval = ChargeUpDuration
    };

    FiringTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = FireDuration
    };

    ChargingUpTimer.Reset();

    ParticleSystem.Play();

    LineRenderer.enabled = true;
    SetLineWidth(0.075f);
  }

  public override void WhileInControl() {
    if ( ChargingUpTimer.Started && ChargingUpTimer.Progress() < StopAimingThreshold )
      Direction = ( (IDirectionManager) DirectionManager ).Direction;

    LineRenderer.SetPosition( 0, transform.position );
    LineRenderer.SetPosition( 1, 1000000 * Direction );

    ChargingUpTimer.IfElapsed( FireLaser );
    FiringTimer.UnlessElapsed( AnimateLaser );
    FiringTimer.IfElapsed( StopFiring );
  }

  void AnimateLaser() {
    SetLineWidth( LineWidth * ( 1 - FiringTimer.Progress() ) );
  }

  void FireLaser() {
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

  public override void ControlRelinquished() {
    LineRenderer.enabled = false;
  }

  void SetLineWidth( float width ) {
    LineRenderer.startWidth = width;
    LineRenderer.endWidth = width;
  }

}
