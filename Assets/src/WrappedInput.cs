using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class WrappedInput : MonoBehaviour {
  public static bool TestMode = false;

  public static Dictionary<string, Nullable<bool>>
    GetButtonOverrides = new Dictionary<string, Nullable<bool>>(),
    GetButtonDownOverrides = new Dictionary<string, Nullable<bool>>(),
    GetButtonUpOverrides = new Dictionary<string, Nullable<bool>>();

  public static Dictionary<string, float?> AxisOverrides = new Dictionary<string, float?>();

  public static Player Player {
    get => ReInput.players.GetPlayer(0);
  }

  public static bool GetButton(string buttonName) {
    return Overridden<bool>(buttonName, GetButtonOverrides) ?? Player.GetButton(buttonName);
  }

  public static bool GetButtonDown(string buttonName) {
    return Overridden<bool>(buttonName, GetButtonDownOverrides) ?? Player.GetButtonDown(buttonName);
  }

  public static bool GetButtonUp(string buttonName) {
    return Overridden<bool>(buttonName, GetButtonUpOverrides) ?? Player.GetButtonUp(buttonName);
  }

  public static float GetAxis(string axis) {
    return Overridden<float>(axis, AxisOverrides) ?? Player.GetAxis(axis);
  }

  public static string ControllerPrefix() {
    if (InputMode.IsJoystick()) {
      return ControllerTypeData.GetControllerType();
    } else {
      return null;
    }
  }

  private static Nullable<T> Overridden<T>(string rawInput, Dictionary<string, Nullable<T>> dict) where T : struct {
    if (!TestMode)
      return null;

    string input = rawInput.ToLower();

    if (dict.ContainsKey(input)) {
      return dict[input];
    }

    return null;
  }
}
