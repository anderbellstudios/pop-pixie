using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadIndicator : MonoBehaviour {
  public MonoBehaviour ButtonHint, EmptyGlow;
  public bool Visible = false;

  void Update() {
    ButtonHint.enabled = Visible;
    EmptyGlow.enabled = Visible;
  }
}
