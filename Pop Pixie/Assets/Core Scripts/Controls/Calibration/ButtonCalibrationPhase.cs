using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonCalibrationPhase : APhase {

  public TMP_Text TopText;
  public TMP_Text BottomText;
  public Image Image;

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
    Image.sprite = CurrentControl.Icon( ControllerTypeData.GetType() );
  }

  public override void WhilePhaseRunning() {
    KeyCode? keyCode = GamePad.AnyButtonDown();

    if ( keyCode != null )
      ButtonPressed( (KeyCode) keyCode );
  }

  void ButtonPressed( KeyCode keyCode ) {
    GamePadButton button = new GamePadButton() {
      Type = GamePadButton.ButtonType.KeyCode,
      KeyCode = keyCode
    };

    GamePadButtonData.SetButton(
      CurrentControl.Name,
      button
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
