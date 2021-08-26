using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : AInspectable {
  public bool IsOpen = false;
  public Transform DoorTransform;
  public List<Transform> MovementPath;
  public float MovementSpeed;

  void Start() {
    if (IsOpen) {
      Open();
    } else {
      Close();
    }

    AInspectableStart();
  }

  public void Open() {
    IsOpen = true;
    SetDoorAngle(90f);
  }

  public void Close() {
    IsOpen = false;
    SetDoorAngle(0f);
  }

  public override void OnInspect() {
    Open();

    GameObject
      .FindWithTag("Player")
      .GetComponent<ScriptedMovement>()
      .FollowPath(
        MovementPath.Select(t => t.position).ToList(),
        MovementSpeed,
        Close
      );
  }

  void SetDoorAngle(float angle) {
    DoorTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
  }

  public override bool IsInspectable() {
    return !IsOpen;
  }

  public override String AInspectablePromptText() {
    return "Press <color=#00ffff>[Interact]</color> to go through door";
  }
}
