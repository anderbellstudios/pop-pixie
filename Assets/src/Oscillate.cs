using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour {
  public Transform Transform;
  public float BaseScale;
  public float OscillationSpeed;
  public float OscillationScale;
  public bool XAxis = true, YAxis = true, IncreaseOnly = false;

  private float Offset;

  void OnEnable() {
    Offset = Time.time;
  }

  void OnDisable() {
    UpdateScale(BaseScale);
  }

  void Update() {
    if (TestMode.Enabled)
      return;

    float c = IncreaseOnly ? 1f : 0f;
    float t = c - Mathf.Cos((Time.time - Offset) * OscillationSpeed);
    float scale = BaseScale + (t * OscillationScale);

    UpdateScale(scale);
  }

  private void UpdateScale(float scale) {
    Transform.localScale = new Vector3(
      XAxis ? scale : 1f,
      YAxis ? scale : 1f,
      1f
    );
  }
}
