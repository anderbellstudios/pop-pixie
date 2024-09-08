using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialInTestMode : MonoBehaviour {
  public SpriteRenderer SpriteRenderer;
  public Material Material;

  void Awake() {
    if (TestMode.Enabled) {
      SpriteRenderer.material = Material;
    }
  }
}
