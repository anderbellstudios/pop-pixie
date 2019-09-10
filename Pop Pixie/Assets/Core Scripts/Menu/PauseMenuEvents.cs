using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuEvents : MonoBehaviour {

  public void Resume() {
    SceneManager.UnloadSceneAsync("Pause Menu");
    StateManager.SetState( State.Playing );
  }

  void Update() {
    if ( WrappedInput.GetButtonDown("Pause") ) {
      Resume();
    }
  }

  public void QuitGame() {
    WrappedApplication.Quit();
  }

}
