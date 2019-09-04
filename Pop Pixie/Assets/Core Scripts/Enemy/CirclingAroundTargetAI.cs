using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclingAroundTargetAI : AEnemyAI, IRequiresLineOfMovementAI {

  public float TooFarThreshold, ApproachToDistance;
  public float TooCloseThreshold, BackOffToDistance;

  public float ApproachSpeed, BackOffSpeed;
  public float CircleSpeed;

  public float AttackInterval;
  public float AttackIntervalRandomness;

  public AEnemyAI WhenAttacking;

  bool AdjustingDistance;
  int CircleDirection;

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

    var random = Random.Range(-1f, 1f);

    if ( random > 0 ) {
      CircleDirection = 1;
    } else {
      CircleDirection = -1;
    }
  }

  public override void WhileInControl() {
    if ( TargetDistance() > TooFarThreshold || TargetDistance() < TooCloseThreshold ) {
      AdjustingDistance = true;
    }

    if ( AdjustingDistance ) {

      if ( TargetDistance() > ApproachToDistance ) {
        ApplyMovement( TargetDirection() * ApproachSpeed );
      } else if ( TargetDistance() < BackOffToDistance ) {
        ApplyMovement( - TargetDirection() * BackOffSpeed );
      } else {
        AdjustingDistance = false;
      }

    } else {

      ApplyMovement(
        Vector2.Perpendicular( TargetDirection() ) * CircleSpeed * CircleDirection
      );

    }

    AttackTimer.IfElapsed(
      () => RelinquishControlTo( WhenAttacking )
    );

  }

  public override void LocalOnCollisionEnter2D( Collision2D _ ) {
    CircleDirection *= -1;
  }

}
