using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeHopper : MonoBehaviour {
  public string SceneName;
  public bool FadeOutMusic = false;

  public void Hop() {
    SceneEvents.Current.ChangeScene(SceneName, FadeOutMusic);
  }
}
