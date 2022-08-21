using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueGameHopper : MonoBehaviour {
  public void Hop() {
    SaveGame.ReadAutoSave();
    SceneData.Load();
  }
}
