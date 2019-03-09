using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfGameStarted : MonoBehaviour {

  public AudioClip Music;
  public ScreenFade Fader;

  void Start () {
    Fader.Fade("from black", 3.0f);
    Invoke("PlayMusic", 3.0f);
  }

  void PlayMusic () {
    MusicController.Current.Play(Music, "thanks for playing");
  }
}
