using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitForAlertAI : AGenericEnemyAI {
  public float MaximumDistance = 10f;
  public bool RequiresLineOfSight = true;
  public UnityEvent OnAlert;

  void Start() {
    Helper.OnDamage(() => {
      OnAlert.Invoke();
    });
  }

  protected override void WhileActive() {
    if (IsAlert()) {
      OnAlert.Invoke();
    }
  }

  private bool IsAlert() {
    if (Helper.DistanceToPlayer > MaximumDistance)
      return false;

    if (RequiresLineOfSight && !Helper.CanSeePlayer())
      return false;

    return true;
  }
}
