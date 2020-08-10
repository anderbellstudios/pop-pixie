using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorEvents : GenericMenuEvents {

  public override void LocalStart() {
    SceneData.Save();
    SaveGame.WriteSave();
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

}
