using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CalibrationAxis {

  public string JoystickName;
  public string AxisName;
  public Sprite Icon;

  public string Name() {
    return JoystickName + " " + AxisName;
  }

}
