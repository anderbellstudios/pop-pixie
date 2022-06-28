using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour {
  public Transform Transform;
  public float BaseScale;
  public float OscillationSpeed;
  public float OscillationScale;

  void Update() {
    Transform.localScale = new Vector3(
      Scale(),
      Scale()
    );
  }

  float Scale() {
    var sine = (float) Math.Sin(Time.time * OscillationSpeed);
    return BaseScale + ( sine * OscillationScale );
  }

}
