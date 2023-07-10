using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMultiHitPointEntity : MonoBehaviour {
  public List<HitPoints> HitPointsList;

  public bool Damage(float damage, bool isCounterAttack = false) {
    foreach (HitPoints hitPoints in HitPointsList) {
      if (hitPoints.Current > 0) {
        return hitPoints.Damage(damage, isCounterAttack);
      }
    }

    return false;
  }
}
