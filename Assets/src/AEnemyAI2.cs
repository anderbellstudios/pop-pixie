using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AEnemyAI2 : MonoBehaviour {
  protected virtual void WhileActive() { }
  protected virtual void OnActivate() { }
  protected virtual void OnDeactivate() { }
  protected virtual void UseChildAIs(Action<AGenericEnemyAI> useChild) { }

  // Used only by AMovementEnemyAI
  protected virtual AMovementEnemyAI InternalUseMovementAI() {
    return null;
  }

  public bool IsActive { get; private set; }
  private List<AEnemyAI2> ActiveChildAIs = new();

  void Update() {
    if (!StateManager.Playing || !IsActive)
      return;
    WhileActive();
    UpdateActiveChildAIs();
  }

  public void UpdateActiveChildAIs() {
    if (!IsActive)
      return;

    List<AEnemyAI2> childAIs = new();

    Action<AEnemyAI2> useChild = (child) => {
      if (!child)
        return;
      if (!ActiveChildAIs.Contains(child)) {
        ActivateChildAI(child);
      }
      childAIs.Add(child);
    };

    UseChildAIs(useChild);
    useChild(InternalUseMovementAI());

    foreach (AEnemyAI2 child in ActiveChildAIs.Except(childAIs)) {
      DeactivateChildAI(child);
    }

    ActiveChildAIs = childAIs;
  }

  protected void Activate() {
    IsActive = true;
    UpdateActiveChildAIs();
    OnActivate();
  }

  protected void Deactivate() {
    OnDeactivate();
    IsActive = false;
    ActiveChildAIs.ForEach(DeactivateChildAI);
    ActiveChildAIs.Clear();
  }

  private void ActivateChildAI(AEnemyAI2 child) {
    child.Activate();
  }

  private void DeactivateChildAI(AEnemyAI2 child) {
    child.Deactivate();
  }
}
