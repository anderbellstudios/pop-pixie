using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSweepingFireAI : AEnemyAI, IDirectionManager {

  public float FireDuration;
  public float FireArc;
  public Turret Turret;
  public AEnemyAI AfterAtack;

  Vector3 CentralDirection;
  public Vector3 Direction { get; set; }

  IntervalTimer FiringTimer;

  public override void ControlGained() {
    FiringTimer = new IntervalTimer() {
      Interval = FireDuration
    };

    FiringTimer.Reset();
    
    CentralDirection = TargetDirection();
  }

  public override void WhileInControl() {
    float dTheta = Mathf.Lerp( -FireArc / 2f, FireArc / 2f, FiringTimer.Progress() );
    Direction = Quaternion.Euler( 0, 0, dTheta ) * CentralDirection;

    Turret.BeginFiring();

    FiringTimer.IfElapsed( StopFiring );
  }

  void StopFiring() {
    Turret.StopFiring();
    RelinquishControlTo( AfterAtack );
  }

}
