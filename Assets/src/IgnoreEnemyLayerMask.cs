using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IgnoreEnemyLayerMask {
  public static int Mask
    => ~LayerMask.GetMask("Enemy", "RollToPassEnemy", "DoNotCollideWithEnemy", "Grenade");
}
