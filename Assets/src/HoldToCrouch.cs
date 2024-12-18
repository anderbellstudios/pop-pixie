using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldToCrouch : MonoBehaviour {
  public Crouch Crouch;

  void Update() {
    if (StateManager.Playing) {
      bool button = WrappedInput.GetButton("Crouch");
      Crouch.TrySetCrouching(button);
    }
  }
}
