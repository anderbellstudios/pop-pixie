using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalibrationController : MonoBehaviour {

  public TMP_Text TopText;
  public TMP_Text BottomText;

  public Image Image;

  public GameObject ControllerOptions;

  public List<CalibrationButton> Controls;

  string ControllerType;
  int ControlIndex = 0;
  CalibrationButton CurrentControl = null;

  void Start() {
    TopText.text = "Controller Calibration";
    BottomText.text = "";

    ControllerOptions.active = true;
  }

  void ChooseController( string type ) {
    ControllerType = type;

    Image.enabled = true;
    ControllerOptions.active = false;

    ControlIndex = 0;
    NextControl();
  }

  void NextControl() {
    if ( ControlIndex == Controls.Count ) {
      CurrentControl = null;
      return;
    }

    CurrentControl = Controls[ ControlIndex ];
    ControlIndex++;

    TopText.text = "Press the button shown below";
    BottomText.text = CurrentControl.Name;
    Image.sprite = CurrentControl.Icon( ControllerType );
  }

  void Update() {
    if ( CurrentControl == null )
      return;

    KeyCode? button = GamePad.AnyButtonDown();

    if ( button != null ) {
      Debug.Log(button);
      NextControl();
    }
  }

  public void ChoosePS() {
    ChooseController("PS");
  }

  public void ChooseXbox() {
    ChooseController("Xbox");
  }

}
