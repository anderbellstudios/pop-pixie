using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class AEnemyAI : MonoBehaviour {
  protected virtual void WhileActive() { }
  protected virtual void OnActivate() { }
  protected virtual void OnDeactivate() { }
  protected virtual void UseChildAIs(Action<AGenericEnemyAI> useChild) { }

  protected virtual bool ShouldAvoidInterruption() => false;

  // Used only by AMovementEnemyAI
  protected virtual AMovementEnemyAI InternalUseMovementAI() {
    return null;
  }

  protected virtual bool InternalMovementAllowed() => false;

  public bool IsActive = false;

  private List<AEnemyAI> ActiveChildAIs = new();
  private EnemyAIHelper _Helper = null;
  private AEnemyAI Parent = null;

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

    List<AEnemyAI> childAIs = new();

    Action<AEnemyAI> useChild = (child) => {
      if (child) {
        childAIs.Add(child);
      }
    };

    UseChildAIs(useChild);
    useChild(InternalUseMovementAI());

    foreach (AEnemyAI child in ActiveChildAIs.Except(childAIs)) {
      DeactivateChildAI(child);
    }

    foreach (AEnemyAI child in childAIs.Except(ActiveChildAIs)) {
      ActivateChildAI(child);
    }

    ActiveChildAIs = childAIs;
  }

  protected void Activate(AEnemyAI parent = null) {
    IsActive = true;
    Parent = parent;
    OnActivate();
    UpdateActiveChildAIs();
  }

  public void Deactivate() {
    IsActive = false;
    ActiveChildAIs.ForEach(DeactivateChildAI);
    ActiveChildAIs.Clear();
    _Helper?.Deactivate();
    OnDeactivate();
  }

  protected virtual void OnChildFinish(AEnemyAI child) {
    OnFinish();
  }

  protected void OnFinish() {
    Parent?.OnChildFinish(this);
  }

  private void ActivateChildAI(AEnemyAI child) {
    child.Activate(this);
  }

  private void DeactivateChildAI(AEnemyAI child) {
    child.Deactivate();
  }

  protected bool AnyDescendant(Func<AEnemyAI, bool> condition) {
    if (condition(this))
      return true;

    return ActiveChildAIs.Any(child => child.AnyDescendant(condition));
  }

  protected bool AvoidingInterruption
    => AnyDescendant(ai => ai.ShouldAvoidInterruption());

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
