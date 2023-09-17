using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramWalls : MonoBehaviour {
  public Renderer Renderer;

  void Awake() {
    Renderer.material.SetFloat("_UnitsWidth", transform.lossyScale.x);
  }
}
