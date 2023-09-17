using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WrappedBaseInput : BaseInput {

  public StandaloneInputModule StandaloneInputModule;

  protected override void Awake() {
    StandaloneInputModule.inputOverride = this;
  }

  public override bool GetButtonDown(string button) {
    if (button == "Horizontal" || button == "Vertical") {
      return base.GetButtonDown(button);
    } else {
      return WrappedInput.GetButtonDown(button);
    }
  }

  public override float GetAxisRaw(string axis) {
    return WrappedInput.GetAxis(axis);
  }

}
