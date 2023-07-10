using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysCounterAttackArbiter : ACounterAttackArbiter {
  public override bool IsCounterAttack() {
    return true;
  }
}
