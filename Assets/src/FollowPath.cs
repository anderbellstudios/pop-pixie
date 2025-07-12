using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FollowPath : MonoBehaviour {
  public ScriptedMovement ScriptedMovement;
  public List<Transform> Anchors;
  public float Speed;
  public bool AvoidCollisions;
  public float AvoidCollisionDistance;
  public UnityEvent OnFinish;

  public void Invoke() {
    ScriptedMovement.FollowPath(
      path: Anchors.Select(anchor => anchor.position).ToList(),
      speed: Speed,
      avoidCollisionDistance: AvoidCollisions ? AvoidCollisionDistance : null,
      onComplete: OnFinish.Invoke
    );
  }
}
