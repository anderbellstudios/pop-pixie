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
  private Action OnComplete;

  public void FollowPath(List<Vector3> path, float speed, Action onComplete) {
    Path = path;
    PathIndex = 0;
    Speed = speed;
    OnComplete = onComplete;
    Running = true;

    if (ScriptedMovementState)
      StateManager.SetState(State.ScriptedMovement);
  }

  void Update() {
    if (!Running || (StateManager.Isnt(State.Playing) && StateManager.Isnt(State.ScriptedMovement)))
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

    MovementManager.Movement += (Vector2) (
      heading.normalized * Mathf.Min(Speed * Time.deltaTime, heading.magnitude)
    );
  }

  void FinishedPath() {
    Running = false;

    if (ScriptedMovementState)
      StateManager.SetState(State.Playing);

    if (OnComplete != null)
      OnComplete();
  }
}
