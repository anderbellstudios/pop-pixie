using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHopper : MonoBehaviour {
  private ButtonPressHelper ButtonPressHelper = new SingleButtonPressHelper();

  void Update() {
    if (!StateManager.Playing) {
      ButtonPressHelper.Clear();
      return;
    }

    if (ButtonPressHelper.GetButtonPress("Pause")) {
      StateManager.AddState(State.Paused);
      SceneManager.LoadScene("Pause Menu", LoadSceneMode.Additive);
    }
  }
}
