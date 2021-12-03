using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingForTargetAI : AEnemyAI {

  public float Speed;
  public int PathfindingElbows = 1;
  public float PathfindingUpdateRate;
  public float PathfindingUpdateRandomness = 1f;

  public AEnemyAI WhenTargetFound;

  IntervalTimer CalculatePathTimer;
  Vector3? NextPoint;

  public override void ControlGained() {
    CalculatePathTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = (1f / PathfindingUpdateRate) + RandomNoise(PathfindingUpdateRandomness)
    };

    CalculatePathTimer.Start();
    
    NextPoint = null;
  }

  public override void WhileInControl() {
    if ( LineOfMovement() ) {
      RelinquishControlTo( WhenTargetFound );
    } else if ( PathfindingElbows > 0 ) {
      NavigateToTarget();
    }
  }

  void NavigateToTarget() {
    if ( !NextPoint.HasValue ) {
      CalculatePathTimer.IfElapsed( CalculatePath );
      return;
    }

    var heading = NextPoint.Value - transform.position;

    ApplyMovement(
      heading.normalized * Speed
    );

    if ( heading.magnitude < 0.4 )
      CalculatePath();

  }

  void CalculatePath() {
    var pathfinder = new AndersonsAlgorithm(
      start:       transform.position,
      destination: Target.transform.position,
      radius: WidthRequiredForMovement() / 2,
      remainingSteps: PathfindingElbows
    );

    NextPoint = pathfinder.NextPoint();
  }

  float RandomNoise(float amplitude) {
    return amplitude * UnityEngine.Random.value;
  }

}
