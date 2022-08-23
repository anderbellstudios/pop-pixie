using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEvents : MonoBehaviour {
  public void TryAgain() {
    SaveGame.ReadSave();
    SceneData.Load();
  }

  public void Quit() {
    WrappedApplication.Quit();
  }
}
