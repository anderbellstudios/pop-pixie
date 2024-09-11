using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleWeaponDirectionManager : MonoBehaviour, IDirectionManager {
  public Vector3 Direction => GetDirection();

  public GameObject Arrow;

  public JoystickWeaponDirectionManager JoystickWeaponDirectionManager;
  public CursorWeaponDirectionManager CursorWeaponDirectionManager;

  private Vector3 CachedDirection = Vector3.up;

  void Update() {
    var rotation = Quaternion.FromToRotation(
      new Vector3(0, 1, 0),
      Direction
    );

    Arrow.transform.rotation = Quaternion.Slerp(
      Arrow.transform.rotation,
      rotation,
      0.3f
    );
  }

  Vector3 GetDirection() {
    if (!StateManager.Playing)
      return CachedDirection;

    Vector3 newDirection = InputMode.IsJoystick()
      ? JoystickWeaponDirectionManager.Direction
      : CursorWeaponDirectionManager.Direction;

    if (newDirection.magnitude == 0)
      return CachedDirection;

    CachedDirection = newDirection;
    return newDirection;
  }
}
