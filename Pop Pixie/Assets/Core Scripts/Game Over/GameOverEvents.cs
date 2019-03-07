using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEvents : MonoBehaviour {

  public ScreenFade Fader;
  public AudioClip Music;

  public void Start () {
    Fader.Fade("from black", 1.0f);
    MusicController.Current.Play(Music, "game over");
  }

  public void TryAgain() {
    Fader.Fade("to black", 2.0f);
    Invoke("ReloadLevel", 2.5f);
  }

  void ReloadLevel () {
    GameDataController.Current.Load();
  }

  public void QuitGame() {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
  }

}
