using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputMode : MonoBehaviour {

  public enum Mode { MouseAndKeyboard, Joystick };
  public static Mode Current = Mode.MouseAndKeyboard;

  void Awake() {
    WrappedInput.Player.AddInputEventDelegate(OnInputUpdate, UpdateLoopType.Update);
  }

  void OnInputUpdate(InputActionEventData inputActionEventData) {
    if (inputActionEventData.IsCurrentInputSource(Rewired.ControllerType.Joystick))
      Current = Mode.Joystick;

    if (inputActionEventData.IsCurrentInputSource(Rewired.ControllerType.Mouse))
      Current = Mode.MouseAndKeyboard;
  }

}