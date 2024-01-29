using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCooldownArbiter : ACanBeDamagedArbiter {
  public float DamageCooldown;

  public override bool CanBeDamaged(HitPoints hp, float damage) {
    return (damage > hp.LastDamageAmount) || ((PlayingTime.time - hp.LastDamaged) > DamageCooldown);
  }
}
