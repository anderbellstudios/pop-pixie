using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGameObjectPhase : APhase {

  public GameObject GameObject;

  public override void LocalBegin() {
    GameObject.active = true;
    PhaseFinished();
  }

}
