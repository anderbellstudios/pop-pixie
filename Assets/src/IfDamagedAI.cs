using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfDamagedAI : AEnemyAI {
  public HitPoints HitPoints;
  public AEnemyAI IfDamaged, IfNotDamaged;

  public override void ControlGained() {
    bool isDamaged = HitPoints.Current < HitPoints.Maximum;
    RelinquishControlTo(isDamaged ? IfDamaged : IfNotDamaged);
  }
}
