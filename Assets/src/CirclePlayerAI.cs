using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePlayerAI : AMovementEnemyAI {
  [field: SerializeField]
  public override float Speed { get; set; }

  public float TooFarDistance, ApproachToDistance;
  public float TooCloseDistance, BackOffToDistance;
  public float CircleSpeed;

  private bool AdjustingDistance;
  private int AdjustingDirection, CircleDirection;
  private LowPriorityBehaviour LowPriorityBehaviour;

  void Start() {
    LowPriorityBehaviour = new LowPriorityBehaviour();

    Helper.OnAnyCollision((Collider2D _) => {
      CircleDirection *= -1;
    });
  }

  protected override void OnActivate() {
    AdjustingDistance = false;

    if (Random.value > 0.5f) {
      CircleDirection = 1;
    } else {
      CircleDirection = -1;
    }
  }

  protected override void WhileActive() {
    float distance = Helper.DistanceToPlayer;

    if (!AdjustingDistance && ShouldStartAdjusting(distance, out AdjustingDirection)) {
      AdjustingDistance = true;
    } else if (AdjustingDistance && ShouldStopAdjusting(distance)) {
      AdjustingDistance = false;
    }

    if (AdjustingDistance) {
      AdjustDistance();
    } else {
      CirclePlayer();
    }
  }

  private bool ShouldStartAdjusting(float distance, out int direction) {
    if (distance < TooCloseDistance) {
      direction = -1;
      return true;
    }

    if (distance > TooFarDistance) {
      direction = 1;
      return true;
    }

    direction = 0;
    return false;
  }

  private bool ShouldStopAdjusting(float distance)
    => distance > BackOffToDistance && distance < ApproachToDistance;

  private void AdjustDistance() {
    Helper.MoveWithVelocity(Helper.DirectionToPlayer * AdjustingDirection * Speed);
  }

  private void CirclePlayer() {
    Vector3 direction = Vector2.Perpendicular(Helper.DirectionToPlayer).normalized * CircleDirection;

    // Check if we're 1 unit away from losing line of movement
    LowPriorityBehaviour.EveryNFrames(10, () => {
      Vector3 testPoint = Helper.Position + direction;
      if (!LineOfMovement.Check(testPoint, Helper.PlayerPosition)) {
        CircleDirection *= -1;
        direction *= -1;
      }
    });

    Helper.MoveWithVelocity(direction * CircleSpeed);
  }
}
