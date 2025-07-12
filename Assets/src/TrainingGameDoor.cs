using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainingGameDoor : AInspectable {
  public Door Door;
  public List<Transform> MovementPath;
  public float MovementSpeed;

  public override void OnInspect() {
    Door.Open();

    PlayerGameObject
      .Current.GetComponent<ScriptedMovement>()
      .FollowPath(MovementPath.Select(t => t.position).ToList(), MovementSpeed, Door.Close);
  }

  public override bool IsInspectable() => !Door.IsOpen;

  public override String AInspectablePromptText() => "Press [Inspect] to go through door";
}
