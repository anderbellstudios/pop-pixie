using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedsDestructiveArbiter : ACanBeDamagedArbiter {
  public override bool CanBeDamaged(HitPoints.DamageContext ctx) {
    return ctx.IsDestructive;
  }
}
