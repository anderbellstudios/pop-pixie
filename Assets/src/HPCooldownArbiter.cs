using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCooldownArbiter : ACanBeDamagedArbiter {
  public float DamageCooldown;

  public override bool CanBeDamaged(HitPoints.DamageContext ctx) {
    if (ctx.Damage > ctx.HitPoints.LastDamageAmount)
      return true;

    float sinceLastDamage = PlayingTime.time - ctx.HitPoints.LastDamaged;
    return sinceLastDamage > DamageCooldown;
  }
}
