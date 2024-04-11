using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptedMovement : MonoBehaviour {
  public MovementManager MovementManager;
  public bool ScriptedMovementState;

  private bool Running = false;
  private List<Vector3> Path;
  private bool SkipAhead = false;
  private float? AvoidCollisionDistance;
  private int PathIndex = 0;
  private float Speed;
  private float DeltaTime;
  private Action OnComplete;
  private int CollisionLayerMask;
  private int FollowPathId = -1;

  private LowPriorityBehaviour LowPriorityBehaviour;

  void Awake() {
    LowPriorityBehaviour = new LowPriorityBehaviour();
  }

  public Action FollowPath(List<Vector3> path, float speed, Action onComplete, bool skipAhead = false, float? avoidCollisionDistance = null) {
    Path = path;
    SkipAhead = skipAhead;
    AvoidCollisionDistance = avoidCollisionDistance;
    PathIndex = 0;
    Speed = speed;
    OnComplete = onComplete;
    Running = true;
    DeltaTime = 0f;

    int currentFollowPathId = ++FollowPathId;

    CollisionLayerMask = CollisionMask.ForLayer(MovementManager.gameObject.layer);

    if (ScriptedMovementState)
      StateManager.AddState(State.ScriptedMovement);

    TrySkipAhead();

    return () => {
      if (currentFollowPathId == FollowPathId) {
        Running = false;
      }
    };
  }

  void Update() {
    if (!StateManager.Enabled(StateFeatures.Movement))
      return;

    if (!Running)
      return;

    if (SkipAhead)
      LowPriorityBehaviour.EveryNFrames(10, TrySkipAhead);

    // Prevent enqueueing movement multiple times per FixedUpdate
    if (MovementManager.Movement != Vector2.zero)
      return;

    DeltaTime += Time.deltaTime;

    Vector3 destination = Path[PathIndex];
    Vector3 direction = destination - transform.position;

    if (direction.magnitude < 0.01) {
      PathIndex++;

      if (PathIndex >= Path.Count) {
        FinishedPath();
        return;
      }
    }

    float avoidCollisionFactor = AvoidCollisionFactor(direction);

    MovementManager.Movement += (Vector2)(
      direction.normalized * Mathf.Min(Speed * avoidCollisionFactor * DeltaTime, direction.magnitude)
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

  /**
   * Check if we have line of movement to each future node in the path,
   * starting with the destination. If so, head straight for that node.
   */
  void TrySkipAhead() {
    for (int i = Path.Count - 1; i > PathIndex; i--) {
      if (LineOfMovement.Check(
        transform.position,
        Path[i],
        layerMask: CollisionLayerMask,
        exclude: MovementManager.gameObject
      )) {
        PathIndex = i;
        return;
      }
    }
  }

  /**
   * If there is a GameObject in our path, slow down until we're
   * AvoidCollisionDistance from it and then stop.
   */
  float AvoidCollisionFactor(Vector3 direction) {
    if (AvoidCollisionDistance == null)
      return 1f;

    GameObject gameObjectAhead = Physics2D.CircleCastAll(
      origin: transform.position,
      radius: 0.5f,
      direction: direction,
      distance: direction.magnitude,
      layerMask: CollisionLayerMask
    )
      .Where(hit => hit.collider.gameObject != MovementManager.gameObject)
      .FirstOrDefault()
      .collider?
      .gameObject;

    if (!gameObjectAhead)
      return 1f;

    float distanceAhead = (gameObjectAhead.transform.position - transform.position).magnitude;
    return 1f - Mathf.Clamp((float)AvoidCollisionDistance / Mathf.Pow(distanceAhead, 2f), 0f, 1.25f);
  }
}
