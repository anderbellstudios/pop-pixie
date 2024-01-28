using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEvents : MonoBehaviour {
  public SavingIndicator SavingIndicator;

  private string NextLevel = "";

  void Start() {
    GameData.Save();

    if (ElevatorData.ArrivedFromLevel)
      SavingIndicator.Saved();
  }

  public void SetNextLevel(string nextLevel) {
    NextLevel = nextLevel;
  }

  public void Continue() {
    SceneEvents.Current.ChangeScene(NextLevel, true);
  }
}
