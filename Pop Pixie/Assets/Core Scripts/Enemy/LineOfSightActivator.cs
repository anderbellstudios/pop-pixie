using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightActivator : AActivator {

  public override bool IsActivated( AEnemyAI ai ) {
    return ai.LineOfMovement(); // Since sight is not implemented yet
  }

}
