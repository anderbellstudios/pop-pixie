using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeOut : MonoBehaviour {
  public bool SingletonInstance = true;
  public static AudioFadeOut Current;

  private Stopwatch Stopwatch = null;
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
    Stopwatch = new Stopwatch.BaseTime();
    Duration = duration * 0.9f; // Prevent pop
    IgnoreMusic = ignoreMusic;

    if (!ignoreMusic) {
      AsyncTimer.BaseTime.SetTimeout(
        () => {
          PlaySong.Stop();
        },
        Duration
      );
    }
  }

  void Update() {
    if (Stopwatch != null) {
      UpdateLevels();
    }
  }

  void UpdateLevels() {
    float volume = 1f - (Stopwatch?.Progress(Duration) ?? 0f);

    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(
      "Master volume",
      AudioManager.ConvertVolumeToParam(IgnoreMusic ? 1f : volume)
    );

    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(
      "Non-music volume",
      AudioManager.ConvertVolumeToParam(IgnoreMusic ? volume : 1f)
    );
  }
}
