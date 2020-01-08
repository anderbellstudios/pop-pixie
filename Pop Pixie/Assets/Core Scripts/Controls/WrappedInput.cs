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
    var joysticks = Input.GetJoystickNames();

    if ( joysticks.Length > 0 ) {

      string joystick = joysticks[0];

      if ( Regex.Match( joystick, @"PLAYSTATION", RegexOptions.IgnoreCase ).Success ) {
        return "PS3";
      }

      if ( Regex.Match( joystick, @"Interactive Entertainment", RegexOptions.IgnoreCase ).Success ) {
        return "PS4";
      }

      if ( Regex.Match( joystick, @"Xbox", RegexOptions.IgnoreCase ).Success ) {
        return "XboxOneS";
      }

      if ( Regex.Match( joystick, @"Unknown Wireless Controller", RegexOptions.IgnoreCase ).Success ) {
        return "PS4";
      }

      Debug.Log( joystick );

    }

    return null;

  }

}
