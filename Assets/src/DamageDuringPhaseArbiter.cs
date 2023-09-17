using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDuringPhaseArbiter : ACanBeDamagedArbiter {

  public APhase Phase;

  public override bool CanBeDamaged(HitPoints hp) {
    return Phase.Running;
  }
}
