using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAccordingToCamera : MonoBehaviour {

  public float WorldScale;

  void Start() {
    Vector2 a = Camera.main.WorldToScreenPoint( new Vector2(0, 0) );
    Vector2 b = Camera.main.WorldToScreenPoint( new Vector2(1, 0) );

    float cameraScale = (b - a).magnitude;
    transform.localScale = Vector3.one * WorldScale * cameraScale;
  }

}
