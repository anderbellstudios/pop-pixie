using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class WrappedInput : MonoBehaviour {

  public static bool GetButton( string button ) {
    return Input.GetButton( GetButtonName(button) );
  }

  public static bool GetButtonDown( string button ) {
    return Input.GetButtonDown( GetButtonName(button) );
  }

  public static string GetButtonName( string button ) {
    return ButtonPrefix() + " " + button;
  }

  static string ButtonPrefix() {
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

    return "Kb+M";

  }

}
