using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

  public static MusicController Current;
  public AudioSource Player;

  IntervalTimer FadeTimer;
  float FadeFrom, FadeTo;

  private string SongName;

  void Awake () {
    if ( Current == null ) {
      Current = this;
    } else {
      Destroy( gameObject );
    }

    DontDestroyOnLoad( gameObject );

    FadeTimer = new IntervalTimer();
  }

  public void Play (AudioClip clip, string songName) { 
    if ( songName == SongName )
      return;

    FadeFrom = 1f;
    FadeTo = 1f;

    Player.clip = clip;
    Player.Play();
    SongName = songName;
  }

  public void Fade(float fadeFrom, float fadeTo, float fadeDuration) {
    FadeFrom = fadeFrom;
    FadeTo = fadeTo;

    FadeTimer.Interval = fadeDuration;
    FadeTimer.Reset();
  }

  void Update () {
    Player.volume = FadeFactor() * StateFactor();
  }

  float FadeFactor() {
    if ( FadeTimer.Started ) {
      return Mathf.Lerp( FadeFrom, FadeTo, FadeTimer.Progress() );
    } 

    return 1f;
  }

  float StateFactor() {
    if ( GameObject.Find("StateManager") != null ) {
      if ( StateManager.Isnt( State.Playing ) ) {
        return 0.25f;
      }
    }

    return 1f;
  }

}
