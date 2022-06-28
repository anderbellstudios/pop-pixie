using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCooldownArbiter : ACanBeDamagedArbiter  {
  public float DamageCooldown;

  public override bool CanBeDamaged(HitPoints hp) {
    var since = DateTime.Now.Subtract(
      hp.LastDamaged
    ).TotalSeconds;

    return since > DamageCooldown;
  }
}
