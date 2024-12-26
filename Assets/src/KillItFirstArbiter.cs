using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillItFirstArbiter : ACanBeDamagedArbiter {
  public GameObject Predecessor;

  public override bool CanBeDamaged(HitPoints.DamageContext ctx) {
    return EnemyUtils.IsDead(Predecessor);
  }
}
