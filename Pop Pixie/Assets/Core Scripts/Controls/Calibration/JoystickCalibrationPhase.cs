using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoystickCalibrationPhase : APhase {

  public TMP_Text TopText;
  public TMP_Text BottomText;
  public Image Image;

  public List<CalibrationAxis> Axes;

  public HashSet<String> AxesThatHaveAtSomePointBeenZero;

  int ControlIndex;
  CalibrationAxis CurrentControl = null;

  public override void LocalBegin() {
    Image.enabled = true;

    AxesThatHaveAtSomePointBeenZero = new HashSet<String>();

    ControlIndex = 0;
    ShowControl();
  }

  void ShowControl() {
    CurrentControl = Axes[ ControlIndex ];

    TopText.text = "Flick the joystick in the direction shown";
    BottomText.text = CurrentControl.JoystickName;
    Image.sprite = CurrentControl.Icon;
  }

  public override void WhilePhaseRunning() {
    float axisValue;

    foreach ( String axis in GamePad.AllAxes() ) {
      axisValue = Input.GetAxis(axis);

      if ( Mathf.Abs( axisValue ) < 0.5 )
        AxesThatHaveAtSomePointBeenZero.Add(axis);

      if ( Mathf.Abs( axisValue ) > 0.5 && AxisHasBeenZero(axis) )
        StickFlicked( axis, axisValue );
    }
  }

  bool AxisHasBeenZero( String axis ) {
    return AxesThatHaveAtSomePointBeenZero.Contains(axis);
  }

  void StickFlicked( String axis, float amount ) {
    AxesThatHaveAtSomePointBeenZero.Clear();

    int sign;

    if ( amount > 0 ) {
      sign = 1;
    } else {
      sign = -1;
    }

    GamePadAxisData.SetAxis(
      CurrentControl.InputName,
      axis,
      sign
    );

    NextControl();
  }

  void NextControl() {
    ControlIndex++;

    if ( ControlIndex == Axes.Count ) {
      PhaseFinished();
    } else {
      ShowControl();
    }

  }

}
