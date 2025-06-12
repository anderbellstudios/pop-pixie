using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEvents : MonoBehaviour {
  public SavingIndicator SavingIndicator;

  void Start() {
    GameData.Save();

    if (ElevatorData.ArrivedFromLevel)
      SavingIndicator.Saved();
  }

  public void PickLevel(string level) {
    LevelCompletionData.PlayedElevatorRide();
    SceneEvents.Current.ChangeScene(level, true);
  }
}
