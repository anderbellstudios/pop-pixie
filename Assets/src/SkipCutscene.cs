using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkipCutscene : MonoBehaviour {
  public bool DebugOnly = true;
  public UnityEvent OnSkip;

  void OnEnable() {
    this.enabled = !DebugOnly || Debug.isDebugBuild;
  }

  void Update() {
    if (WrappedInput.GetButtonDown("Pause")) {
      OnSkip.Invoke();
    }
  }
}
