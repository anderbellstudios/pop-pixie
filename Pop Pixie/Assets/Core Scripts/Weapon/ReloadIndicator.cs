using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadIndicator : MonoBehaviour {
  public MonoBehaviour Indicator;
  public bool Visible = false;

  void Update() {
    Indicator.enabled = Visible;
  }

}
