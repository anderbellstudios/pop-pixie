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

  private bool AdjustingDistance;
  private int CircleDirection;

  public override void ControlGained() {
    float duration = AttackInterval + (Random.Range(-1f, 1f) * AttackIntervalRandomness / 2f);

    SetTimeout(() => {
      RelinquishControlTo(WhenAttacking);
    }, duration);

    if (Random.value > 0.5f) {
      CircleDirection = 1;
    } else {
      CircleDirection = -1;
    }
  }

  public override void WhileInControl() {
    if (TargetDistance() > TooFarThreshold || TargetDistance() < TooCloseThreshold) {
      AdjustingDistance = true;
    }

    if (AdjustingDistance) {
      AdjustDistance();
    } else {
      Circle();
    }
  }

  private void AdjustDistance() {
    if (TargetDistance() > ApproachToDistance) {
      ApplyMovement(TargetDirection() * ApproachSpeed);
    } else if (TargetDistance() < BackOffToDistance) {
      ApplyMovement(-TargetDirection() * BackOffSpeed);
    } else {
      AdjustingDistance = false;
    }
  }

  private void Circle() {
    ApplyMovement(
      Vector2.Perpendicular(TargetDirection()) * CircleSpeed * CircleDirection
    );
  }

  public override void LocalOnCollisionEnter2D(Collision2D _) {
    CircleDirection *= -1;
  }
}
