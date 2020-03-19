using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorWeaponDirectionManager : MonoBehaviour, IDirectionManager {

  public float Threshold;

  private Vector3 previousMousePosition = Vector3.zero;

  public bool Active() => (previousMousePosition - Input.mousePosition).magnitude >= Threshold;

  public Vector3 Direction {
    get => RawDirection().normalized;
  }

  private Vector3 RawDirection() {
    var mousePosition = Input.mousePosition;
    previousMousePosition = mousePosition;

    var onscreenMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

    return new Vector3(
      onscreenMousePosition.x - transform.position.x, 
      onscreenMousePosition.y - transform.position.y, 
      0
    );
  }

}
