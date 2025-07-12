using System;
using System.Collections;
using System.Collections.Generic;
using KoganeUnityLib;
using TMPro;
using UnityEngine;

public class CutsceneTextManager : MonoBehaviour {
  public bool SingletonInstance = true;
  public static CutsceneTextManager Current;

  public TMP_Text Text;
  public TMP_Typewriter Typewriter;

  private Stopwatch FadeOutStopwatch = null;
  private float FadeOutDuration;
  private Action OnFadeOut;

  void Awake() {
    if (SingletonInstance)
      Current = this;
  }

  public void Write(
    string text,
    float totalDuration,
    float typewriterDuration,
    float fadeOutDuration,
    Action onComplete
  ) {
    SetOpacity(1);

    float fadeOutDelay = totalDuration - typewriterDuration - fadeOutDuration;

    if (fadeOutDelay < 0) {
      throw new Exception("Not enough time for fade out on line: " + text);
    }

    Typewriter.Play(
      text: text,
      speed: text.Length / typewriterDuration,
      onComplete: () => {
        AsyncTimer.BaseTime.SetTimeout(
          () => {
            FadeOut(fadeOutDuration, onComplete);
          },
          fadeOutDelay
        );
      }
    );
  }

  void FadeOut(float duration, Action onFadeOut) {
    FadeOutStopwatch = new Stopwatch.BaseTime();
    FadeOutDuration = duration;
    OnFadeOut = onFadeOut;
  }

  void Update() {
    if (FadeOutStopwatch != null) {
      float progress = FadeOutStopwatch.Progress(FadeOutDuration);
      SetOpacity(1f - progress);

      if (progress >= 1f) {
        FadeOutStopwatch = null;
        SetOpacity(0f);
        OnFadeOut();
      }
    }
  }

  void SetOpacity(float opacity) {
    Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, opacity);
  }
}
