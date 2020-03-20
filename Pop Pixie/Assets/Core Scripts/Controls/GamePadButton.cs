using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GamePadButton {

  // Some controllers implement buttons as axis
  public enum ButtonType { KeyCode, Axis };

  public ButtonType Type;
  public KeyCode KeyCode;
  public string AxisName;

  public bool GetButton() {
    switch ( Type ) {
      case ButtonType.KeyCode:
        return Input.GetKey(KeyCode);

      case ButtonType.Axis:
        return Input.GetAxis(AxisName) > 0;

      default:
        Debug.LogError("Invalid button type " + Type);
        return false;
    }
  }

  private bool previousAxisState = false;

  public bool GetButtonDown() {
    switch ( Type ) {
      case ButtonType.KeyCode:
        return Input.GetKeyDown(KeyCode);

      case ButtonType.Axis:
        bool currentState = GetButton();
        bool result = !previousAxisState && currentState;
        previousAxisState = currentState;
        return currentState;

      default:
        Debug.LogError("Invalid button type " + Type);
        return false;
    }
  }

}
