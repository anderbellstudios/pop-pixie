using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickWeaponDirectionManager : MonoBehaviour, IDirectionManager {

  public Vector3 Direction {
    get => RawDirection().normalized;
  }

  private Vector3 RawDirection() => new Vector3(
    WrappedInput.GetAxis("Fire X"),
    WrappedInput.GetAxis("Fire Y"),
    0
  );

}
