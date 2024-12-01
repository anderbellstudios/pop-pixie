using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishInstantlyAI : AMovementEnemyAI {
  protected override void OnActivate() {
    OnFinish();
  }
}
