using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageHitPointsInRadius {
  public static bool Invoke(
    float damage,
    Vector3 origin,
    float radius,
    bool canBeCounterAttacked = false,
    bool isDestructive = false,
    AnimationCurve damageCurve = null,
    Func<GameObject, bool> shouldDamage = null
  ) {
    bool isCounterAttack = false;

    foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>()) {
      HitPoints hitPoints = gameObject.GetComponent<HitPoints>();

      if (hitPoints == null)
        continue;

      if (shouldDamage != null && !shouldDamage(gameObject))
        continue;

      Collider2D collider = gameObject.GetComponent<Collider2D>();

      /**
       * If the game object doesn't have a collider, use its centre. If the
       * game object has a collider that touches the origin, use a distance of
       * 0. Otherwise, use the closest point on the collider to the origin.
       */
      Vector3 closestPoint = collider == null
        ? gameObject.transform.position
        : collider.OverlapPoint(origin)
          ? origin
          : collider.ClosestPoint(origin);

      float distance = (closestPoint - origin).magnitude;

      if (distance <= radius) {
        float damageMultiplier = damageCurve == null
          ? 1f
          : damageCurve.Evaluate(distance / radius);

        isCounterAttack |= hitPoints.Damage(
          damage * damageMultiplier,
          canBeCounterAttacked: canBeCounterAttacked,
          isDestructive: isDestructive
        );
      }
    }

    return isCounterAttack;
  }
}
