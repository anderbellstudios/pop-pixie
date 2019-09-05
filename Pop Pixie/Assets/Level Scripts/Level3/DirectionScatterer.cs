using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionScatterer : MonoBehaviour, IDirectionManager {
  public MonoBehaviour OriginalDirectionManager;
  public float Angle;

  public Vector3 Direction {
    get { return ScatteredDirection(); }
    set {} // <-- I get the feeling this is what the interface segregation principle was designed to avoid. 
  }

  Vector3 ScatteredDirection() {
    var originalDirection = ( (IDirectionManager) OriginalDirectionManager ).Direction;
    return RandomRotation() * originalDirection;
  }

  Quaternion RandomRotation() {
    float angle = Random.Range( -Angle, Angle ) / 2;
    return Quaternion.Euler(0, 0, angle);
  }
}
