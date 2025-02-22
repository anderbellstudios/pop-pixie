using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HologramSplashWalls : MonoBehaviour {
  public Image Image;

  void Awake() {
    if (TestMode.Enabled) {
      Image.material.SetFloat("_Speed", 0f);
    }
  }
}
