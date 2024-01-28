using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEvents : MonoBehaviour {
  private string NextLevel = "";

  void Start() {
    GameData.Save();
  }

  public void SetNextLevel(string nextLevel) {
    NextLevel = nextLevel;
  }

  public void Continue() {
    SceneEvents.Current.ChangeScene(NextLevel, true);
  }
}
