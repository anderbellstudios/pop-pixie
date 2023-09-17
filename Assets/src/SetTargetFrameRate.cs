using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetFrameRate : MonoBehaviour {

  public int FrameRate;
  private float MovingAverageFrameRate;

  void Start() {
    Application.targetFrameRate = FrameRate;
    InvokeRepeating("LogFrameRate", 5, 5);
  }

  void Update() {
    MovingAverageFrameRate = (MovingAverageFrameRate * 0.9f) + (0.1f / Time.deltaTime);
  }

  void LogFrameRate() {
    EnhancedDataCollection.LogIfEnabled(() => "Frame rate: " + MovingAverageFrameRate);
  }

}
