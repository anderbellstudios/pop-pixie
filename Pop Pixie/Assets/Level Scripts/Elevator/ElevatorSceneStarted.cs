using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorSceneStarted : MonoBehaviour {

  public ScreenFade Fader;

  // Start is called before the first frame update
  void Start() {
    Fader.Fade("from black", 2.0f);

    Invoke("FadeToNextLevel", 8.0f);
  }

  void FadeToNextLevel () {
    Fader.Fade("to black", 4.0f);
    AudioMixer.Current.FadeOut(4.0f);
    Invoke("NextLevel", 5.0f);
  }

  void NextLevel () {
    GDCall.ExpectFirstTime();

    SceneManager.LoadScene(
      ElevatorData.NextLevel
    );
  }

}
