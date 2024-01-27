using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorWeaponDirectionManager : MonoBehaviour, IDirectionManager {
  public Vector3 Direction {
    get => CursorDirection.DirectionFromWorldPoint(transform.position).normalized;
  }
}
