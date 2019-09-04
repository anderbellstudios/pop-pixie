using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyActivator : AActivator {

  public bool Activated;

  public override bool IsActivated() {
    return Activated;
  }

}
