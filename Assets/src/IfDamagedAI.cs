using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfDamagedAI : AMovementEnemyAI {
  public AMovementEnemyAI IfDamaged, IfNotDamaged;

  protected override AMovementEnemyAI UseMovementAI()
    => IsDamaged() ? IfDamaged : IfNotDamaged;

  private bool IsDamaged()
    => Helper.HitPoints.Current < Helper.HitPoints.Maximum;
}
