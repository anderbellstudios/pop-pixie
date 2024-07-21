using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeOut : MonoBehaviour {
  public bool SingletonInstance = true;
  public static AudioFadeOut Current;

  private float StartTime = -1f;
  private float Duration = 1f;
  private bool IgnoreMusic = true;

  void Awake() {
    if (SingletonInstance)
      Current = this;
  }

  void Start() {
    UpdateLevels();
  }

  public void FadeOut(float duration, bool ignoreMusic = true) {
    StartTime = Time.time;
    Duration = duration * 0.9f; // Prevent pop
    IgnoreMusic = ignoreMusic;

    PlaySong currentSong = PlaySong.Current;
    if (!ignoreMusic && currentSong) {
      AsyncTimer.BaseTime.SetTimeout(() => {
        PlaySong.Current.Stop();
      }, Duration);
    }
  }

  void Update() {
    if (StartTime >= 0f) {
      UpdateLevels();
    }
  }

  float GetVolume() {
    if (StartTime < 0f) return 1f;
    float progress = (Time.time - StartTime!) / Duration;
    return 1f - Mathf.Clamp(progress, 0f, 1f);
  }

  void UpdateLevels() { 
    float volume = GetVolume();
    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Master volume", IgnoreMusic ? 1f : volume);
    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Non-music volume", IgnoreMusic ? volume : 1f);
  }
}

