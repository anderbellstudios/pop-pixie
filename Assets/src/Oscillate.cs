using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour {
  public Transform Transform;
  public float BaseScale;
  public float OscillationSpeed;
  public float OscillationScale;
  public bool XAxis = true, YAxis = true;

  void Update() {
    if (TestMode.Enabled)
      return;

    float t = Mathf.Sin(Time.time * OscillationSpeed);
    float scale = BaseScale + (t * OscillationScale);

    Transform.localScale = new Vector3(
      XAxis ? scale : 1f,
      YAxis ? scale : 1f,
      1f
    );
  }
}
