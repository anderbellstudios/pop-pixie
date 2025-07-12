using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightActivator : AActivator {
  public override bool IsActivated(ALegacyEnemyAI ai) {
    return ai.LineOfMovement(); // Since sight is not implemented yet
  }
}
