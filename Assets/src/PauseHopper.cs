using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHopper : MonoBehaviour {

  public bool SaveOnPause = true;

  private ButtonPressHelper ButtonPressHelper = new SingleButtonPressHelper();

  void Update() {
    if (!StateManager.Playing) {
      ButtonPressHelper.Clear();
      return;
    }

    if (ButtonPressHelper.GetButtonPress("Pause")) {
      if (SaveOnPause) {
        SceneData.Save();
        SaveGame.WriteAutoSave();
      }

      StateManager.AddState(State.Paused);
      SceneManager.LoadScene("Pause Menu", LoadSceneMode.Additive);
    }
      
  }

}
