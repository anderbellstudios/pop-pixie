using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForActivationAI : AEnemyAI {

  public AActivator Activator;
  public AEnemyAI WhenActivated;

  public override void WhileInControl() {
    if (Activator.IsActivated(this)) {
      RelinquishControlTo(WhenActivated);
    }
  }

}
