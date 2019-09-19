using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

  public static MusicController Current;
  public AudioSource Player;

  private float BaseVolume;
  private float FadeFrom, FadeTo, FadeDuration, FadeProgress, FadeFactor;
  private float StateFactor;

  private string SongName;

  void Awake () {
    if ( Current == null ) {
      Current = this;
    } else {
      Destroy( gameObject );
    }

    DontDestroyOnLoad( gameObject );

    BaseVolume = 0f;
  }

  public void Play (AudioClip clip, string songName) { 
    if ( songName == SongName )
      return;

    BaseVolume = 1f;
    FadeFrom = 1f;
    FadeTo = 1f;

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
    FadeFactor = Mathf.Lerp(
      FadeFrom,
      FadeTo,
      FadeProgress / FadeDuration
    );

    SetStateFactor();

    Debug.Log("they're");
    Debug.Log(BaseVolume);
    Debug.Log(FadeFactor);
    Debug.Log(StateFactor );
    Player.volume = BaseVolume * FadeFactor * StateFactor;

    FadeProgress += Time.deltaTime;
  }

  void SetStateFactor() {
    StateFactor = 1f;

    if ( GameObject.Find("StateManager") != null ) {
      if ( StateManager.Isnt( State.Playing ) ) {
        StateFactor = 0.25f;
      }
    }
  }

}
