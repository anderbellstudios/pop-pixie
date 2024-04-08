using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NavigateToPoint : MonoBehaviour {
  public ScriptedMovement ScriptedMovement;
  public UnityEvent OnComplete;
  public float Speed = 10f;
  public float AvoidCollisionDistance = 4f;
  public float RecomputeInterval = 3f;
  public Vector3 DestinationPoint;

  private bool AfterStart = false;
  private int GraphVersion = 0;
  private AsyncTimer.EnqueuedEvent Timer;

  void Start() {
    AfterStart = true;
    OnEnable();
  }

  void OnEnable() {
    if (!AfterStart)
      return;
    RecomputePath();
    Timer = AsyncTimer.PlayingTime.SetInterval(RecomputePath, RecomputeInterval);
  }

  void OnDisable() {
    AsyncTimer.PlayingTime.ClearTimeout(Timer);
    // TODO: Interrupt scripted motion
  }

  void Update() {
    if (GraphVersion < PathfindingGraph.Current.RecomputeVersion) {
      RecomputePath();
    }
  }

  void RecomputePath() {
    PathfindingGraph graph = PathfindingGraph.Current;
    if (!graph)
      throw new System.Exception("NavigateToPoint requires a PathfindingGraph to exist in the scene");

    GraphVersion = graph.RecomputeVersion;

    List<Vector3> path = graph.FindPath(transform.position, DestinationPoint);

    if (path != null) {
      ScriptedMovement.FollowPath(
        path: path,
        speed: Speed,
        onComplete: OnComplete.Invoke,
        skipAhead: true,
        avoidCollisionDistance: AvoidCollisionDistance
      );
    }
  }
}
