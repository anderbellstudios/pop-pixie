using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneEventsCallbacks : MonoBehaviour {
  public bool CallInAwake = false;
  public UnityEvent OnFirstTime, OnRetry;

  void Awake() {
    if (CallInAwake)
      CallCallbacks();
  }

  void Start() {
    if (!CallInAwake)
      CallCallbacks();
  }

  private void CallCallbacks() {
    if (SceneEvents.Current.IsRetry) {
      OnRetry.Invoke();
    } else {
      OnFirstTime.Invoke();
    }
  }
}
