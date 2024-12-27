using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCanBeDamagedArbiter : ACanBeDamagedArbiter {
  [field: SerializeField]
  public bool Invulnerable { get; set; }

  public override bool CanBeDamaged(HitPoints.DamageContext ctx) {
    return !Invulnerable;
  }
}
