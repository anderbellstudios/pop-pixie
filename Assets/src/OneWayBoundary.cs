using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OneWayBoundary : MonoBehaviour {
  public bool Locked;
  public Behaviour LockBehaviour;
  public UnityEvent OnCrossed;

  bool AwaitingCross = false;

  void Awake() {
    if (Locked) {
      Lock();
    } else {
      Unlock();
    }

    AwaitingCross = !Locked;
  }

  public void Lock() {
    LockBehaviour.enabled = true;
    HandleCrossed(); // In case the player clips through the first boundary
    Locked = true;
  }

  public void Unlock() {
    LockBehaviour.enabled = false;
    AwaitingCross = true;
    Locked = false;
  }

  public void HandleCrossed() {
    if (AwaitingCross) {
      OnCrossed.Invoke();
      AwaitingCross = false;
    }
  }
}
