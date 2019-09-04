using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingForTargetAI : AEnemyAI {

  public float Speed;
  public int PathfindingElbows = 1;
  public float PathfindingUpdateRate;

  public AEnemyAI WhenTargetFound;

  IntervalTimer CalculatePathTimer;
  Vector3[] Path;

  public override void ControlGained() {
    CalculatePathTimer = new IntervalTimer() {
      Interval = 1f / PathfindingUpdateRate
    };

    CalculatePathTimer.Start();
    
    Path = null;
  }

  public override void WhileInControl() {
    if ( LineOfMovement() ) {
      RelinquishControlTo( WhenTargetFound );
    } else if ( PathfindingElbows > 0 ) {
      NavigateToTarget();
    }
  }

  void NavigateToTarget() {
    if ( Path == null ) {
      CalculatePathTimer.IfElapsed( CalculatePath );
      return;
    }

    var heading = Path[0] - transform.position;

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

    Path = pathfinder.Vertices();
  }

}
