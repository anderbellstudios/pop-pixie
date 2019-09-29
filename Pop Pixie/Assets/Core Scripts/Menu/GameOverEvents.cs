using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEvents : GenericMenuEvents {

  public void TryAgain() {
    FadeOut(_TryAgain);
  }

  public void QuitGame() {
    FadeOut(_QuitGame);
  }

  void _TryAgain () {
    GameData.ReadSave();

    GameData.Load();
  }

  void _QuitGame() {
    WrappedApplication.Quit();
  }

}
