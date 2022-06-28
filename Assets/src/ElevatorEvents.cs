using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ElevatorEvents : GenericMenuEvents {

  public TMP_Text AssistModeButtonText;

  EnumeratorButton<AssistModeData.AssistModeHP> AssistModeButton;

  public override void LocalStart() {
    SceneData.Save();
    SaveGame.WriteSave();

    AssistModeButton = AssistModeData.MakeAssistModeButton(
      (label) => { AssistModeButtonText.text = label; }
    );
  }

  public void Continue () {
    FadeOut( () => {
      GDCall.ExpectFirstTime();
      SceneManager.LoadScene(ElevatorData.NextLevel);
    });
  }

  public void LoadShop() {
    FadeOut( () => SceneManager.LoadScene("Shop") );
  }

  public void QuitGame() {
    FadeOut(WrappedApplication.Quit);
  }

  public void ShiftAssistMode() {
    AssistModeButton.Shift();
  }

}
