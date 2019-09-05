using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadIndicator : MonoBehaviour {
  public Image IndicatorImage;
  public bool Visible = false;

  void Update() {
    IndicatorImage.enabled = Visible;
  }

}
