using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingForTargetAI : AEnemyAI {
  public NavigateToPoint NavigateToPoint;
  public AEnemyAI WhenTargetFound;

  public override void ControlGained() {
    UpdateNavigateToPoint();
    NavigateToPoint.enabled = true;
  }

  public override void WhileInControl() {
    UpdateNavigateToPoint();

    if (LineOfMovement()) {
      RelinquishControlTo(WhenTargetFound);
    }
  }

  public override void ControlRelinquished() {
    NavigateToPoint.enabled = false;
  }

  void UpdateNavigateToPoint() {
    NavigateToPoint.DestinationPoint = Target.transform.position;
  }
}
