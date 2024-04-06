using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class WrappedInput : MonoBehaviour {
  public static bool TestMode = false;

  public static Dictionary<string, bool>
    GetButtonOverrides = new Dictionary<string, bool>(),
    GetButtonDownOverrides = new Dictionary<string, bool>(),
    GetButtonUpOverrides = new Dictionary<string, bool>();

  public static Player Player {
    get => ReInput.players.GetPlayer(0);
  }

  public static bool GetButton(string buttonName) {
    return Overridden(buttonName, GetButtonOverrides) || Player.GetButton(buttonName);
  }

  public static bool GetButtonDown(string buttonName) {
    return Overridden(buttonName, GetButtonDownOverrides) || Player.GetButtonDown(buttonName);
  }

  public static bool GetButtonUp(string buttonName) {
    return Overridden(buttonName, GetButtonUpOverrides) || Player.GetButtonUp(buttonName);
  }

  public static float GetAxis(string axis) {
    return Player.GetAxis(axis);
  }

  public static string ControllerPrefix() {
    if (InputMode.IsJoystick()) {
      return ControllerTypeData.GetControllerType();
    } else {
      return null;
    }
  }

  private static bool Overridden(string rawButtonName, Dictionary<string, bool> dict) {
    if (!TestMode)
      return false;
    string buttonName = rawButtonName.ToLower();
    return dict.ContainsKey(buttonName) && dict[buttonName];
  }
}
