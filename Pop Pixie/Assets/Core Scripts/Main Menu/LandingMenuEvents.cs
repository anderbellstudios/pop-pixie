using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingMenuEvents : MonoBehaviour {

  public AudioClip Music;
  public ScreenFade Fader;

  void Start () {
    Fader.Fade("to black", 0.0f);
    MusicController.Current.Play(Music, "landing menu");
    Invoke("FadeIn", 4.0f);
  }

  void FadeIn () {
    Fader.Fade("from black", 8.0f);
  }

  public void Begin() {
    MusicController.Current.Fade(1.0f, 0.0f, 4.0f);
    Fader.Fade("to black", 4.0f);
    Invoke("MainMenu", 5.0f);
  }

  void MainMenu () {
    SceneManager.LoadScene("Main Menu");
  }

}
