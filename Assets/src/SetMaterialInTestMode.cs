using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialInTestMode : MonoBehaviour {
  public static bool TestMode;

  public SpriteRenderer SpriteRenderer;
  public Material Material;

  void Awake() {
    if (TestMode) {
      SpriteRenderer.material = Material;
    }
  }
}
