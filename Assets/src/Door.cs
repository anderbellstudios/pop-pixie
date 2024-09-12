using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : AInspectable {
  public bool IsOpen = false;
  public PlaySound PlaySound;
  public Transform DoorTransform;
  public BoxCollider2D DoorCollider;
  public List<Transform> MovementPath;
  public float MovementSpeed;

  void Start() {
    SetIsOpen(IsOpen);
    PlaySound.Play();
    AInspectableStart();
  }

  public void Open() => SetIsOpen(true);
  public void Close() => SetIsOpen(false);

  void SetIsOpen(bool isOpen) {
    IsOpen = isOpen;
    PlaySound.EventInstance.setParameterByName("Is Open", isOpen ? 1f : 0f);
    SetDoorAngle(isOpen ? 90f : 0f);
    SetCollider(!isOpen);
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
