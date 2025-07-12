using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerInTestMode : MonoBehaviour {
  public UnityEvent OnAwake,
    OnStart;

  void Awake() {
    if (TestMode.Enabled) {
      OnAwake.Invoke();
    }
  }

  void Start() {
    if (TestMode.Enabled) {
      OnStart.Invoke();
    }
  }
}
