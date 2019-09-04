using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostLineOfMovementInterrupt : AInterrupt {

  public AEnemyAI WhenLostLineOfMovement;

  public override Type OnlyAIsMatching() {
    return typeof( IRequiresLineOfMovementAI );
  }

  public override bool ShouldInterrupt( AEnemyAI ai ) {
    return !ai.LineOfMovement();
  }

  public override AEnemyAI InterruptAI() {
    return WhenLostLineOfMovement;
  }

}
