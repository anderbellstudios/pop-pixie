using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentPhase : APhase {

  public MonoBehaviour Component;

  public override void LocalBegin() {
    Component.enabled = true;
    PhaseFinished();
  }

}
