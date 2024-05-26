using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEvents : MonoBehaviour {
  public void TryAgain() {
    string level = GameOverData.ResumeLevel;

    if (level == "") {
      throw new System.Exception("ResumeLevel cannot be empty");
    }

    GameData.LoadOrReset();
    GameOverData.IsRetry = true;
    SceneEvents.Current.ChangeScene(level, true, true);
  }
}
