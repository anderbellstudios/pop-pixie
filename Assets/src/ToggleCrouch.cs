using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCrouch : MonoBehaviour {
  public Crouch Crouch;

  private ButtonPressHelper ButtonPressHelper = new SingleButtonPressHelper();

  void Start() {
    InGamePrompt.Current.RegisterSource(
      InGamePrompt.Priority.Uncrouch,
      () => Crouch.Crouching ? "Press [Crouch] to stop crouching" : null
    );
  }

  void Update() {
    if (StateManager.Playing) {
      if (ButtonPressHelper.GetButtonPress("Crouch")) {
        Crouch.TrySetCrouching(!Crouch.Crouching);
      }
    } else {
      ButtonPressHelper.Clear();
    }
  }
}
