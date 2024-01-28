using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueGameHopper : MonoBehaviour {
  public bool HopOnStart = false;

  void Start() {
    if (HopOnStart)
      Hop();
  }

  public void Hop() {
    GameData.Load();
    ElevatorData.FromShop = false;
    SceneEvents.Current.ChangeScene("Elevator", true);
  }
}
