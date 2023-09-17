using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedMovement : MonoBehaviour {
  public MovementManager MovementManager;
  public bool ScriptedMovementState;

  private bool Running = false;
  private List<Vector3> Path;
  private int PathIndex = 0;
  private float Speed;
  private float DeltaTime;
  private Action OnComplete;

  public void FollowPath(List<Vector3> path, float speed, Action onComplete) {
    Path = path;
    PathIndex = 0;
    Speed = speed;
    OnComplete = onComplete;
    Running = true;
    DeltaTime = 0f;

    if (ScriptedMovementState)
      StateManager.AddState(State.ScriptedMovement);
  }

  void Update() {
    if (!StateManager.Enabled(StateFeatures.Movement))
      return;

    if (!Running)
      return;

    DeltaTime += Time.deltaTime;

    // Prevent enqueueing movement multiple times per FixedUpdate
    if (MovementManager.Movement != Vector2.zero)
      return;

    Vector3 destination = Path[PathIndex];
    Vector3 heading = destination - transform.position;

    if (heading.magnitude < 0.01) {
      PathIndex++;

      if (PathIndex >= Path.Count) {
        FinishedPath();
        return;
      }
    }

    MovementManager.Movement += (Vector2)(
      heading.normalized * Mathf.Min(Speed * DeltaTime, heading.magnitude)
    );

    DeltaTime = 0f;
  }

  void FinishedPath() {
    Running = false;

    if (ScriptedMovementState)
      StateManager.RemoveState(State.ScriptedMovement);

    if (OnComplete != null)
      OnComplete();
  }
}
