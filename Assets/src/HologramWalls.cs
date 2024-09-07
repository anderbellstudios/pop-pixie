using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramWalls : MonoBehaviour {
  public static bool TestMode = false;

  public Renderer Renderer;

  void Awake() {
    Renderer.material.SetFloat("_UnitsWidth", transform.lossyScale.x);

    if (TestMode) {
      Renderer.material.SetFloat("_Speed", 0f);
    }
  }
}
