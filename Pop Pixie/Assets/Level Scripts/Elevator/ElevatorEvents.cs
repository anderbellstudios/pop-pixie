using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorEvents : GenericMenuEvents, ISerializableComponent {

  public string[] SerializableFields { get; } = { "NextLevel" };

  public string NextLevel;

  public override void LocalStart() {
    if (ElevatorData.NextLevel != null) {
      NextLevel = ElevatorData.NextLevel;
    }

    SceneData.Save();
    SaveGame.WriteSave();
  }

  public void Continue () {
    FadeOut( () => {
      GDCall.ExpectFirstTime();
      SceneManager.LoadScene(NextLevel);
    });
  }

  public void LoadShop() {
    FadeOut( () => SceneManager.LoadScene("Shop") );
  }

  public void QuitGame() {
    FadeOut(WrappedApplication.Quit);
  }

}
