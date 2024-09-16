using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KoganeUnityLib;

public class CutsceneTextManager : MonoBehaviour {
  public bool SingletonInstance = true;
  public static CutsceneTextManager Current;

  public TMP_Text Text;
  public TMP_Typewriter Typewriter;

  private bool FadingOut;
  private float FadeOutStartTime, FadeOutDuration;
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
        AsyncTimer.BaseTime.SetTimeout(() => {
          FadeOut(fadeOutDuration, onComplete);
        }, fadeOutDelay);
      }
    );
  }

  void FadeOut(float duration, Action onFadeOut) {
    FadingOut = true;
    FadeOutStartTime = Time.time;
    FadeOutDuration = duration;
    OnFadeOut = onFadeOut;
  }

  void Update() {
    if (FadingOut) {
      float progress = Mathf.Clamp01((Time.time - FadeOutStartTime) / FadeOutDuration);
      SetOpacity(1f - progress);

      if (progress >= 1f) {
        FadingOut = false;
        SetOpacity(0f);
        OnFadeOut();
      }
    }
  }

  void SetOpacity(float opacity) {
    Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, opacity);
  }
}
