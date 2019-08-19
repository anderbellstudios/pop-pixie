using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : AFireable {

  public override void BeginFiring() {
    Debug.Log("So what am I, uh, supposed to do here?");
  }

  public override void StopFiring() {
    Debug.Log("Well, I gave it everything I could. Can't ask for more than that.");
  }

}
