using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class WrappedInput : MonoBehaviour {

  public static bool GetButton( string button ) {
    KeyCode? controllerButton = ControllerButtonKeyCode(button);

    if ( controllerButton != null ) {
      if ( Input.GetKey( (KeyCode) controllerButton ) )
        return true;
    }

    return Input.GetButton( KeyboardButtonName(button) );
  }

  public static bool GetButtonDown( string button ) {
    KeyCode? controllerButton = ControllerButtonKeyCode(button);

    if ( controllerButton != null ) {
      if ( Input.GetKeyDown( (KeyCode) controllerButton ) )
        return true;
    }

    return Input.GetButtonDown( KeyboardButtonName(button) );
  }

  public static float GetAxis( string axis ) {
    float amount;

    if ( axis == "Horizontal" || axis == "Vertical" ) {
      amount = Input.GetAxis( axis );
    } else {
      amount = 0;
    }

    String controllerInput = GamePadAxisData.GetInput( axis );
    int controllerSign = GamePadAxisData.GetSign( axis );

    if ( controllerInput != null ) {
      float controllerAmount = Input.GetAxis( controllerInput ) * controllerSign;

      if ( Mathf.Abs( controllerAmount ) > Mathf.Abs(amount) )
        amount = controllerAmount;
    }

    return amount;
  }

  static KeyCode? ControllerButtonKeyCode( string button ) {
    return GamePadButtonData.GetKeyCode( button );
  }

  static string KeyboardButtonName( string button ) {
    return "Kb+M " + button;
  }

  public static string ControllerPrefix() {
    if ( Input.GetJoystickNames().Length > 0 ) {
      return ControllerTypeData.GetType();
    } else {
      return null;
    }
  }

}
