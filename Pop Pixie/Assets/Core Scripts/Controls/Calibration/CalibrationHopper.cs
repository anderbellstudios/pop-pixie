using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalibrationHopper : MonoBehaviour {

  static bool AlreadyAsked = false;

  void Start() {
    InvokeRepeating( "Check", 1f, 1f );
  }

  void Check() {
    if ( AlreadyAsked )
      return;

    if ( StateManager.Isnt( State.Playing ) )
      return;

    if ( Input.GetJoystickNames().Length == 0 )
      return;

    if ( ControllerTypeData.GetType() != null )
      return;

    AlreadyAsked = true;

    StateManager.SetState( State.Paused );
    SceneManager.LoadScene( "Controller Calibration", LoadSceneMode.Additive );
  }

}
