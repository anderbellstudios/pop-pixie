using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEvents : MonoBehaviour {

  public void TryAgain() {
    GameDataController.Current.Load();
  }

  public void QuitGame() {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
  }

}
