using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeOut : MonoBehaviour {
  public bool SingletonInstance = true;
  public static AudioFadeOut Current;

  private IntervalTimer FadeOutTimer;
  private bool IgnoreMusic = false;

  void Awake() {
    if (SingletonInstance)
      Current = this;

    FadeOutTimer = new IntervalTimer();
  }

  public void FadeOut(float duration, bool ignoreMusic = true) {
    FadeOutTimer.Interval = duration;
    FadeOutTimer.Reset();
    IgnoreMusic = ignoreMusic;
  }

  public float FadeLevel(bool isMusic = false) {
    return FadeOutTimer.Started && (!isMusic || !IgnoreMusic)
      ? 1f - FadeOutTimer.Progress()
      : 1f;
  }
}
