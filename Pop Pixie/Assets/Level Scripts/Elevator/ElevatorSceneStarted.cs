using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSceneStarted : MonoBehaviour {

  public ScreenFade Fader;

  // Start is called before the first frame update
  void Start() {
    Fader.Fade("from black", 2.0f);
    MusicController.Current.SetVolume(0.25f);

    Invoke("FadeToNextLevel", 8.0f);
  }

  void FadeToNextLevel () {
    Fader.Fade("to black", 4.0f);
    Invoke("NextLevel", 5.0f);
  }

  void NextLevel () {
    Debug.Log("Need to load next level");
  }

}
