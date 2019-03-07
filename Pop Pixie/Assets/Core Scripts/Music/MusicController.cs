using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

  public static MusicController Current;
  public AudioSource Player;

  private float FadeFrom, FadeTo, FadeDuration, FadeProgress;

  private string SongName;

  void Awake () {
    if ( Current == null ) {
      Current = this;
    } else {
      Destroy( gameObject );
    }

    DontDestroyOnLoad( gameObject );

    SetVolume(0.0f);
  }

  void SetVolume (float volume) {
    FadeFrom = volume;
    FadeTo = volume;
    FadeDuration = 1.0f;
    FadeProgress = 1.0f;
  }

  public void Play (AudioClip clip, string songName) { 
    if ( songName == SongName )
      return;

    SetVolume(1.0f);
    Player.clip = clip;
    Player.Play();
    SongName = songName;
  }

  public void Fade(float fadeFrom, float fadeTo, float fadeDuration) {
    FadeFrom = fadeFrom;
    FadeTo = fadeTo;
    FadeDuration = fadeDuration;
    FadeProgress = 0.0f;
  }

  void Update () {
    Player.volume = Mathf.Lerp(
      FadeFrom,
      FadeTo,
      FadeProgress / FadeDuration
    );

    FadeProgress += Time.deltaTime;
  }

}
