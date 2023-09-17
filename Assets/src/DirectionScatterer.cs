using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionScatterer : MonoBehaviour, IDirectionManager {
  public MonoBehaviour OriginalDirectionManager;
  public float Angle;

  void Awake() {
    Debug.Log("DirectionScatterer is deprecated. Please use ScatterDirection instead.");
  }

  public Vector3 Direction {
    get => ScatteredDirection();
  }

  Vector3 ScatteredDirection() {
    var originalDirection = ((IDirectionManager)OriginalDirectionManager).Direction;
    return RandomRotation() * originalDirection;
  }

  Quaternion RandomRotation() {
    float angle = Random.Range(-Angle, Angle) / 2;
    return Quaternion.Euler(0, 0, angle);
  }
}
