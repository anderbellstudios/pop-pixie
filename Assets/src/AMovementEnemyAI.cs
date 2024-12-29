using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMovementEnemyAI : AEnemyAI {
  public virtual float Speed {
    get {
      throw new System.NotImplementedException("Cannot get Speed on AI that doesn't implmenet it");
    }
    set {
      Debug.LogWarning("Tried to set Speed on AI that doesn't implement it");
    }
  }

  protected virtual AMovementEnemyAI UseMovementAI() {
    return null;
  }

  protected override bool InternalMovementAllowed() => true;
  protected override AMovementEnemyAI InternalUseMovementAI() => UseMovementAI();
}
