using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadIndicator : MonoBehaviour {
  public Image IndicatorImage;
  public bool Visible = false;
  public float BaseScale;
  public float OscillationSpeed;
  public float OscillationScale;

  void Update() {
    IndicatorImage.enabled = Visible;
    IndicatorImage.transform.localScale = new Vector3(
      Scale(),
      Scale()
    );
  }

  float Scale() {
    var sine = (float) Math.Sin(Time.time * OscillationSpeed);
    return BaseScale + ( sine * OscillationScale );
  }

}
