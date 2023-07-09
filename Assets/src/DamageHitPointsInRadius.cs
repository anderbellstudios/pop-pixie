using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageHitPointsInRadius {
  public static bool Invoke(
    float damage,
    Vector3 origin,
    float radius,
    bool canBeCounterAttacked = false,
    AnimationCurve damageCurve = null
  ) {
    bool isCounterAttack = false;

    foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>()) {
      HitPoints hp = go.GetComponent<HitPoints>();
      float distance = (go.transform.position - origin).magnitude;

      if (hp != null && distance <= radius) {
        float damageMultiplier = damageCurve == null ? 1 : damageCurve.Evaluate(distance / radius);

        if (hp.Damage(damage * damageMultiplier, canBeCounterAttacked)) {
          isCounterAttack = true;
        }
      }
    }

    return isCounterAttack;
  }
}
