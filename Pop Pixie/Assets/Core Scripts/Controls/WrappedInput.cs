using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Rewired;

public class WrappedInput : MonoBehaviour {

  private static Player Player {
    get => ReInput.players.GetPlayer(0);
  }

  public static bool GetButton( string buttonName ) {
    return Player.GetButton( buttonName );
  }

  public static bool GetButtonDown( string buttonName ) {
    return Player.GetButtonDown( buttonName );
  }

  public static float GetAxis( string axis ) {
    return Player.GetAxis( axis );
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
