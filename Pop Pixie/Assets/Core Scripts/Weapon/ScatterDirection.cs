using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScatterDirection {
  public static Vector3 Scatter(Vector3 direction, float amount) {
    float angle = Random.Range(-amount, amount) / 2;
    Quaternion quaternion = Quaternion.Euler(0, 0, angle);
    return quaternion * direction;
  }
}
