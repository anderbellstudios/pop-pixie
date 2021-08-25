using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorWeaponDirectionManager : MonoBehaviour, IDirectionManager {

  public Vector3 Direction {
    get => RawDirection().normalized;
  }

  private Vector3 RawDirection() {
    var mousePosition = Input.mousePosition;
    var onscreenMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

    return new Vector3(
      onscreenMousePosition.x - transform.position.x, 
      onscreenMousePosition.y - transform.position.y, 
      0
    );
  }

}
