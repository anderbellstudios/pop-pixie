using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OneWayBoundary : MonoBehaviour {
  public bool LockedOnAwake;
  public Behaviour LockBehaviour;
  public UnityEvent OnCrossed;

  bool AwaitingCross = false;

  void Awake() {
    if (LockedOnAwake) {
      Lock();
    } else {
      Unlock();
    }

    AwaitingCross = !LockedOnAwake;
  }

  public void Lock() {
    LockBehaviour.enabled = true;
    HandleCrossed(); // In case the player clips through the first boundary
  }

  public void Unlock() {
    LockBehaviour.enabled = false;
    AwaitingCross = true;
  }

  public void HandleCrossed() {
    if (AwaitingCross) {
      OnCrossed.Invoke();
      AwaitingCross = false;
    }
  }
}
