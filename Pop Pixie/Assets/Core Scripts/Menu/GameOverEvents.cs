using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverEvents : GenericMenuEvents {

  public Text AssistModeButtonText;

  EnumeratorButton<AssistModeData.AssistModeHP> AssistModeButton;

  public override void LocalStart() {
    AssistModeButton = AssistModeData.MakeAssistModeButton(
      (label) => { AssistModeButtonText.text = label; }
    );
  }

  public void TryAgain() {
    FadeOut(_TryAgain);
  }

  public void QuitGame() {
    FadeOut(_QuitGame);
  }

  public void ShiftAssistMode() {
    AssistModeButton.Shift();
  }

  void _TryAgain () {
    SaveGame.ReadSave();

    SceneData.Load();
  }

  void _QuitGame() {
    WrappedApplication.Quit();
  }

}
