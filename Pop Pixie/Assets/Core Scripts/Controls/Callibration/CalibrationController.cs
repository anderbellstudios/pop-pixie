using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CalibrationController : MonoBehaviour {

  public TMP_Text TopText;
  public TMP_Text BottomText;

  public Image Image;

  public GameObject ControllerOptions;

  public List<CalibrationButton> Controls;

  public Sprite DoneImage;

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
      ShowDoneScreen();
      return;
    }

    CurrentControl = Controls[ ControlIndex ];
    ControlIndex++;

    TopText.text = "Press the button shown below";
    BottomText.text = CurrentControl.Name;
    Image.sprite = CurrentControl.Icon( ControllerType );
  }

  void Update() {
    if ( Input.GetKeyDown( KeyCode.Escape ) ) {
      SceneManager.UnloadSceneAsync("Controller Calibration");
      StateManager.SetState( State.Playing );
      return;
    }

    if ( CurrentControl == null )
      return;

    KeyCode? keyCode = GamePad.AnyButtonDown();

    if ( keyCode != null ) {
      GamePadButtonData.SetKeyCode(
        CurrentControl.Name,
        (KeyCode) keyCode
      );

      Debug.Log( GamePadButtonData.GetKeyCode( CurrentControl.Name ) );

      NextControl();
    }
  }

  void ShowDoneScreen() {
    TopText.text = "Calibration complete";
    BottomText.text = "Press escape to exit.";

    Image.sprite = DoneImage;
  }

  public void ChoosePS() {
    ChooseController("PS");
  }

  public void ChooseXbox() {
    ChooseController("Xbox");
  }

}
