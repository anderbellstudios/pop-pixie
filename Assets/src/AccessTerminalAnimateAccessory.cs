using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AccessTerminalAnimateAccessory : MonoBehaviour {
  public bool HopOnStart = true;
  public Transform Accessory;
  public int DeltaPixels;
  public float Duration;
  public UnityEvent OnFinish;

  private AsyncTimer.EnqueuedEvent EnqueuedEvent;

  void Start() {
    if (HopOnStart)
      Hop();
  }

  public void Hop() {
    int absDeltaPixels = System.Math.Abs(DeltaPixels);
    int direction = DeltaPixels >= 0 ? 1 : -1;

    EnqueuedEvent = AsyncTimer.BaseTime.SetInterval(() => {
      if (absDeltaPixels-- > 0) {
        Accessory.localPosition += Vector3.up * direction;
      } else {
        AsyncTimer.BaseTime.ClearTimeout(EnqueuedEvent);
        OnFinish.Invoke();
      }
    }, Duration / absDeltaPixels);
  }
}
