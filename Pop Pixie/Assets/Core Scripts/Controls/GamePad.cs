using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePad {

  public static KeyCode? AnyButtonDown() {
    foreach ( KeyCode button in AllButtons() ) {
      if ( Input.GetKeyDown(button) )
        return button;
    }

    return null;
  }

  public static KeyCode[] AllButtons() {
    return new KeyCode[] {
      KeyCode.JoystickButton0,
      KeyCode.JoystickButton1,
      KeyCode.JoystickButton2,
      KeyCode.JoystickButton3,
      KeyCode.JoystickButton4,
      KeyCode.JoystickButton5,
      KeyCode.JoystickButton6,
      KeyCode.JoystickButton7,
      KeyCode.JoystickButton8,
      KeyCode.JoystickButton9,
      KeyCode.JoystickButton10,
      KeyCode.JoystickButton11,
      KeyCode.JoystickButton12,
      KeyCode.JoystickButton13,
      KeyCode.JoystickButton14,
      KeyCode.JoystickButton15,
      KeyCode.JoystickButton16,
      KeyCode.JoystickButton17,
      KeyCode.JoystickButton18,
      KeyCode.JoystickButton19
    };
  }

  public static String[] AllAxes() {
    return new String[] {
      "Joystick Axis 0",
      "Joystick Axis 1",
      "Joystick Axis 2",
      "Joystick Axis 3",
      "Joystick Axis 4",
      "Joystick Axis 5",
      "Joystick Axis 6",
      "Joystick Axis 7",
      "Joystick Axis 8",
      "Joystick Axis 9",
      "Joystick Axis 10",
      "Joystick Axis 11",
      "Joystick Axis 12",
      "Joystick Axis 13",
      "Joystick Axis 14",
      "Joystick Axis 15",
      "Joystick Axis 16",
      "Joystick Axis 17",
      "Joystick Axis 18",
      "Joystick Axis 19",
      "Joystick Axis 20",
      "Joystick Axis 21",
      "Joystick Axis 22",
      "Joystick Axis 23",
      "Joystick Axis 24",
      "Joystick Axis 25",
      "Joystick Axis 26",
      "Joystick Axis 27"
    };
  }

}
