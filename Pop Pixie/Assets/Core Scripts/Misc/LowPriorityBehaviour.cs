using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPriorityBehaviour {
  int Offset;
  bool FirstTime;

  public LowPriorityBehaviour() {
    Offset = UnityEngine.Random.Range(0, 100);
    FirstTime = true;
  }

  public void EveryNFrames(int n, Action action) {
    if (FirstTime || (Time.frameCount + Offset) % n == 0) {
      action();
      FirstTime = false;
    }
  }
}
