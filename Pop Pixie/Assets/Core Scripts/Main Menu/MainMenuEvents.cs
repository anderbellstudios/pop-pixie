﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEvents : MonoBehaviour {

  public AudioClip Music;
  public ScreenFade Fader;

  void Start () {
    MusicController.Current.Play(Music, "main menu");
    Fader.Fade("from black", 2.0f);
  }

  public void NewGame () {
    FadeOutAndRun("NewGameInstant");
  }

  void NewGameInstant () {
    GameDataController.Current.NewGame();
  }

  public void Continue () {
    FadeOutAndRun("ContinueInstant");
  }

  public void ContinueInstant () {
    GameDataController.Current.Load();
  }

  void FadeOutAndRun (string callback) {
    Fader.Fade("to black", 3.0f);
    MusicController.Current.Fade(1.0f, 0.0f, 3.0f);
    Invoke(callback, 4.0f);
  }

}
