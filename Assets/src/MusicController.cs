using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
  public SeamlessAudioSource SeamlessAudioSource;
  public Interpolator FadeInterpolator;

  public bool SingletonInstance = true;
  public static MusicController Current;

  Song CurrentSong = null;

  void Awake() {
    if (SingletonInstance) {
      if (Current != null) {
        Destroy(gameObject);
        return;
      }

      Current = this;
      DontDestroyOnLoad(gameObject);
    }

    SetFadeLevel(1f);
  }

  public void Play(Song song) {
    if (song.Equals(CurrentSong))
      return;

    if (song.HasIntro) {
      if (song.Resume)
        throw new System.Exception("Resume is not supported for songs with intro");

      SeamlessAudioSource.Play(song.IntroClip, () => {
        HandleSongStarted(song);

        /**
         * Delaying scheduling the main clip seems to improve the accuracy
         * of the transition for songs started immediately on scene load.
         */
        AsyncTimer.BaseTime.SetTimeout(() => {
          if (CurrentSong == song)
            SeamlessAudioSource.PlayNext(song.AudioClip, null);
        }, Mathf.Min(1f, song.IntroClip.length / 2f));
      });
    } else {
      AudioSource audioSource = SeamlessAudioSource.Play(song.AudioClip, () => {
        HandleSongStarted(song);
      });

      if (song.Resume)
        audioSource.timeSamples = SongPlaybackTimeData.Fetch(song);
    }
  }

  public void PlayNext(Song song) {
    if (song == null) {
      throw new System.Exception("Cannot call PlayNext with null");
    }

    if (song.HasIntro) {
      throw new System.Exception("PlayNext does not support songs with intro");
    }

    if (song.Resume) {
      throw new System.Exception("PlayNext does not support resume");
    }

    SeamlessAudioSource.PlayNext(song.AudioClip, () => {
      CurrentSong = song;
    });
  }

  public void Stop() {
    CurrentSong = null;
    SeamlessAudioSource.Stop();
  }

  public void SetFadeLevel(float level) {
    FadeInterpolator.Reset(level);
  }

  public void FadeTo(float level, float duration) {
    FadeInterpolator.Animate(level, duration);
  }

  public void FadeOut(float duration) {
    FadeTo(0f, duration);
  }

  public void FadeIn(float duration) {
    FadeTo(1f, duration);
  }

  void Update() {
    float volume =
      AudioFadeOut.Current.FadeLevel(true)
      * FadeInterpolator.Evaluate()
      * ((float)OptionsData.MusicVolume);

    SeamlessAudioSource.ForEachAudioSource((audioSource) => {
      audioSource.volume = volume;
    });

    if (CurrentSong != null && CurrentSong.Resume)
      SongPlaybackTimeData.Record(CurrentSong, SeamlessAudioSource.PrimaryAudioSource.timeSamples);
  }

  void HandleSongStarted(Song song) {
    CurrentSong = song;
    if (song.ResetFadeLevel)
      SetFadeLevel(1f);
  }
}
