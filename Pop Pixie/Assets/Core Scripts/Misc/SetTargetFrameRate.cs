using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetFrameRate : MonoBehaviour {

  public int FrameRate;

  void Start() {
    Application.targetFrameRate = FrameRate;
  }

}
