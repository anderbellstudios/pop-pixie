using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonCalibrationPhase : APhase {

  public TMP_Text TopText;
  public TMP_Text BottomText;
  public Image Image;

  public ControllerTypeSelectionPhase ControllerTypeSelectionPhase;

  public List<CalibrationButton> Buttons;

  int ControlIndex;
  CalibrationButton CurrentControl = null;

  public override void LocalBegin() {
    Image.enabled = true;

    ControlIndex = 0;
    ShowControl();
  }

  void ShowControl() {
    CurrentControl = Buttons[ ControlIndex ];

    TopText.text = "Press the button shown below";
    BottomText.text = CurrentControl.Name;
    Image.sprite = CurrentControl.Icon( ControllerTypeSelectionPhase.ControllerType );
  }

  public override void WhilePhaseRunning() {
    KeyCode? keyCode = GamePad.AnyButtonDown();

    if ( keyCode != null )
      ButtonPressed( (KeyCode) keyCode );
  }

  void ButtonPressed( KeyCode keyCode ) {
    GamePadButtonData.SetKeyCode(
      CurrentControl.Name,
      (KeyCode) keyCode
    );

    NextControl();
  }

  void NextControl() {
    ControlIndex++;

    if ( ControlIndex == Buttons.Count ) {
      PhaseFinished();
    } else {
      ShowControl();
    }

  }

}
