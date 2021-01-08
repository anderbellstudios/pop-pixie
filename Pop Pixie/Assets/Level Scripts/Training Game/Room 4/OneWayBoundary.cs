using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OneWayBoundary : MonoBehaviour {
  public bool LockedOnAwake;
  public Behaviour LockBehaviour;
  public UnityEvent OnCrossed;

  void Awake() {
    if (LockedOnAwake) {
      Lock();
    } else {
      Unlock();
    }
  }

  public void Lock() {
    LockBehaviour.enabled = true;
  }

  public void Unlock() {
    LockBehaviour.enabled = false;
  }

  public void HandleCrossed() {
    OnCrossed.Invoke();
  }
}
