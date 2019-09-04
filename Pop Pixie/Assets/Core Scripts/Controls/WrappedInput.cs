using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class WrappedInput : MonoBehaviour {

  public static bool GetButton( string button ) {
    var controllerButton = ControllerButtonName(button);

    if ( controllerButton != null ) {
      if ( Input.GetButton(controllerButton) )
        return true;
    }

    return Input.GetButton( KeyboardButtonName(button) );
  }

  public static bool GetButtonDown( string button ) {
    var controllerButton = ControllerButtonName(button);

    if ( controllerButton != null ) {
      if ( Input.GetButtonDown(controllerButton) )
        return true;
    }

    return Input.GetButtonDown( KeyboardButtonName(button) );
  }

  static string ControllerButtonName( string button ) {
    var prefix = ControllerPrefix();

    if ( prefix != null )
      return ControllerPrefix() + " " + button;

    return null;
  }

  static string KeyboardButtonName( string button ) {
    return "Kb+M " + button;
  }

  static string ControllerPrefix() {
    var joysticks = Input.GetJoystickNames();

    if ( joysticks.Length > 0 ) {

      string joystick = joysticks[0];

      if ( Regex.Match( joystick, @"PLAYSTATION", RegexOptions.IgnoreCase ).Success ) {
        return "PS3";
      }

      if ( Regex.Match( joystick, @"Xbox", RegexOptions.IgnoreCase ).Success ) {
        return "XboxOneS";
      }

    }

    return null;

  }

}
