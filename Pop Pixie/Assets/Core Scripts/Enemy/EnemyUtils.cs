using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUtils {
  public static bool IsDead( GameObject enemyGO ) 
    => enemyGO == null || GetDeadFlag(enemyGO);

  static bool GetDeadFlag( GameObject enemyGO ) 
    => enemyGO.GetComponent<EnemyHitPointEvents>().IsDead;
}
