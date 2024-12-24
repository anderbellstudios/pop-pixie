using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUtils {
  public static bool IsDead(GameObject enemyGO)
    => enemyGO == null || GetDeadFlag(enemyGO);

  public static List<GameObject> InContainer(Transform container) {
    return GameObject.FindGameObjectsWithTag("Enemy").Where(enemy => {
      if (container == null)
        return true;
      return enemy.transform.IsChildOf(container);
    }).ToList();
  }

  private static bool GetDeadFlag(GameObject enemyGO)
    => enemyGO.GetComponent<HitPoints>().Dead;
}
