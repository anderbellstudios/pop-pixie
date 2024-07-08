using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragUtils {
  public static float MaxDisplacement(float speed, float drag)
    => speed / drag;

  public static Vector3 VelocityForDisplacement(
    Vector3 displacement,
    float drag
  ) => drag * displacement;

  public static float TimeUntilDisplacement(float speed, float drag, float displacement) {
    float time = -1f * (1f / drag) * Mathf.Log(1f - (drag * displacement) / speed);

    if (float.IsInfinity(time)) {
      Debug.LogError("TimeUntilDisplacement got Infinity");
    }

    return time;
  }
}
