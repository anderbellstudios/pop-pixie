using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingMenuEvents : MonoBehaviour {

  public AudioClip Music;
  public ScreenFade Fader;
  public MonoBehaviour Button;

  public float FadeInDelay, FadeInDuration, FadeOutDuration, NextSceneDelay;

  void Start () {
    DisableButton();
    Fader.Fade("to black", 0.0f);
    MusicController.Current.Play(Music, "landing menu");
    Invoke("FadeIn", FadeInDelay);
  }

  void FadeIn () {
    Fader.Fade("from black", FadeInDuration);
    Invoke("EnableButton", FadeInDuration);
  }


  public void Begin() {
    DisableButton();
    MusicController.Current.Fade(1.0f, 0.0f, FadeOutDuration);
    Fader.Fade("to black", FadeOutDuration);
    Invoke("MainMenu", FadeOutDuration + NextSceneDelay);
  }

  void MainMenu () {
    SceneManager.LoadScene("Main Menu");
  }

  void EnableButton () {
    Button.enabled = true;
  }

  void DisableButton () {
    Button.enabled = false;
  }
}
