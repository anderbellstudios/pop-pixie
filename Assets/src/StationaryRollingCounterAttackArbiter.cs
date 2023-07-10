using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryRollingCounterAttackArbiter : ACounterAttackArbiter {
  public Roll Roll;

  public override bool IsCounterAttack() {
    return Roll.IsStationaryRolling();
  }
}
