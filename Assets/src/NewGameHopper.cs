using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameHopper : MonoBehaviour {
  public SceneChangeHopper SceneChangeHopper;

  public void Hop() {
    GameData.Reset();
    SceneChangeHopper.Hop();
  }
}
