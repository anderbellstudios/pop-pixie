using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeHopper : MonoBehaviour {
  public string SceneName;
  public bool FadeOutMusic = false;
  public float OverrideFadeOutDuration = -1f;

  public void Hop() {
    SceneEvents.Current.ChangeScene(
      SceneName,
      fadeOutMusic: FadeOutMusic,
      overrideFadeOutDuration: OverrideFadeOutDuration
    );
  }
}
