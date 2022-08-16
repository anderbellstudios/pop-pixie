using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : AInspectable {
  public bool IsOpen = false;
  public SoundHopper OpenSound, CloseSound;
  public Transform DoorTransform;
  public BoxCollider2D DoorCollider;
  public List<Transform> MovementPath;
  public float MovementSpeed;

  void Start() {
    if (IsOpen) {
      Open(true);
    } else {
      Close(true);
    }

    AInspectableStart();
  }

  public void Open(bool isStart) {
    IsOpen = true;
    SetDoorAngle(90f);
    SetCollider(false);

    if (!isStart)
      OpenSound.Hop();
  }

  public void Open() => Open(false);

  public void Close(bool isStart = false) {
    IsOpen = false;
    SetDoorAngle(0f);
    SetCollider(true);

    if (!isStart)
      CloseSound.Hop();
  }

  public void Close() => Close(false);

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

  void SetCollider(bool enabled) {
    DoorCollider.enabled = enabled;
  }

  public override bool IsInspectable() {
    return !IsOpen;
  }

  public override String AInspectablePromptText() {
    return "Press [Inspect] to go through door";
  }
}
