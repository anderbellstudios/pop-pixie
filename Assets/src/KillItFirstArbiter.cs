using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillItFirstArbiter : ACanBeDamagedArbiter {
  public GameObject Predecessor;

  public override bool CanBeDamaged(HitPoints hp, float damage) {
    return EnemyUtils.IsDead(Predecessor);
  }
}
