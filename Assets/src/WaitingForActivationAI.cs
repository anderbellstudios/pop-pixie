using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForActivationAI : ALegacyEnemyAI {

  public AActivator Activator;
  public ALegacyEnemyAI WhenActivated;

  public override void WhileInControl() {
    if (Activator.IsActivated(this)) {
      RelinquishControlTo(WhenActivated);
    }
  }

}
