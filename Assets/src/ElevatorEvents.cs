using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEvents : MonoBehaviour {
  void Start() {
    SceneData.Save();
    SaveGame.WriteSave();
  }

  public void Continue() {
    GDCall.ExpectFirstTime();
    SceneEvents.Current.ChangeScene(ElevatorData.NextLevel, true);
  }
}
