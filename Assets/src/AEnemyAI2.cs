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

  protected virtual bool InternalMovementAllowed() => false;

  public bool IsActive = false;

  private List<AEnemyAI2> ActiveChildAIs = new();
  private EnemyAIHelper _Helper = null;

  protected EnemyAIHelper Helper {
    get {
      if (_Helper == null) {
        _Helper = MakeHelper();
      }
      return _Helper;
    }
  }

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
      if (child) {
        childAIs.Add(child);
      }
    };

    UseChildAIs(useChild);
    useChild(InternalUseMovementAI());

    foreach (AEnemyAI2 child in ActiveChildAIs.Except(childAIs)) {
      DeactivateChildAI(child);
    }

    foreach (AEnemyAI2 child in childAIs.Except(ActiveChildAIs)) {
      ActivateChildAI(child);
    }

    ActiveChildAIs = childAIs;
  }

  protected void Activate() {
    IsActive = true;
    OnActivate();
    UpdateActiveChildAIs();
  }

  protected void Deactivate() {
    IsActive = false;
    ActiveChildAIs.ForEach(DeactivateChildAI);
    ActiveChildAIs.Clear();
    OnDeactivate();
  }

  private void ActivateChildAI(AEnemyAI2 child) {
    child.Activate();
  }

  private void DeactivateChildAI(AEnemyAI2 child) {
    child.Deactivate();
  }

  private EnemyAIHelper MakeHelper() => new EnemyAIHelper(
    ai: this,
    gameObject: GetRootTransform().gameObject,
    movementAllowed: InternalMovementAllowed()
  );

  // Find the first ancestor tagged "Enemy"
  private Transform GetRootTransform() {
    Transform enemy = null;
    Transform current = transform;

    while (current != null) {
      if (current.tag == "Enemy") {
        enemy = current;
      }
      current = current.parent;
    }

#if UNITY_EDITOR
    Debug.Assert(enemy != null, "Failed to find root transform");
#endif

    return enemy;
  }
}
