using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMovementEnemyAI : AEnemyAI2 {
  protected virtual AMovementEnemyAI UseMovementAI() {
    return null;
  }

  protected override AMovementEnemyAI InternalUseMovementAI() => UseMovementAI();
}
