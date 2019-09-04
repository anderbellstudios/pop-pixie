using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingBackAI : AEnemyAI {

  public float Speed;
  public int PathfindingElbows;

  public AEnemyAI WhenArrivedBack;
  public AEnemyAI WhenTargetFound;

  Vector3 OriginalPosition;

  Vector3[] Path;

  public override void LocalStart() {
    OriginalPosition = transform.position;
  }

  public override void ControlGained() {
    CalculatePath();
  }

  public override void WhileInControl() {
    if ( LineOfMovement() ) {
      RelinquishControlTo( WhenTargetFound );
      return;
    }

    if ( ( OriginalPosition - transform.position ).magnitude  < 0.1 ) {
      RelinquishControlTo( WhenArrivedBack );
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
      destination: OriginalPosition,
      radius: WidthRequiredForMovement() / 2,
      remainingSteps: PathfindingElbows
    );

    Path = pathfinder.Vertices();
  }

}
