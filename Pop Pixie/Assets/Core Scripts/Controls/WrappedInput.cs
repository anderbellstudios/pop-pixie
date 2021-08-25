using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Rewired;

public class WrappedInput : MonoBehaviour {

  public static Player Player {
    get => ReInput.players.GetPlayer(0);
  }

  public static bool GetButton( string buttonName ) {
    return Player.GetButton( buttonName );
  }

  public static bool GetButtonDown( string buttonName ) {
    return Player.GetButtonDown( buttonName );
  }

  public static bool GetButtonUp( string buttonName ) {
    return Player.GetButtonUp( buttonName );
  }

  public static float GetAxis( string axis ) {
    return Player.GetAxis( axis );
  }

  public static string ControllerPrefix() {
    if (InputMode.IsJoystick()) {
      return ControllerTypeData.GetControllerType();
    } else {
      return null;
    }
  }

}
