using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CalibrationController : MonoBehaviour {

  public PhaseScheduler PhaseScheduler;

  void Start() {
    PhaseScheduler.InitPhases();
  }

  void Update() {
    if ( Input.GetKeyDown( KeyCode.Escape ) ) {
      SceneManager.UnloadSceneAsync("Controller Calibration");
      StateManager.SetState( State.Playing );
    }
  }

}
