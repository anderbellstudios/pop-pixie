using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoAttachCameraToCanvas : MonoBehaviour {
  public Canvas Canvas;

  void Awake() {
    Canvas.worldCamera = Camera.main;
  }
}
