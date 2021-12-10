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

  private IntervalTimer FadeOutTimer;
  private Action OnFadeOut;

  void Awake () {
    if (SingletonInstance)
      Current = this;

    FadeOutTimer = new IntervalTimer();
  }

  public void Write(string text, float duration, Action onComplete) {
    SetOpacity(1);

    Typewriter.Play(
      text: text,
      speed: text.Length / duration,
      onComplete: onComplete
    );
  }

  public void FadeOut(float duration, Action onFadeOut) {
    FadeOutTimer.Interval = duration;
    FadeOutTimer.Reset();
    OnFadeOut = onFadeOut;
  }

  void Update() {
    if (FadeOutTimer.Started) {
      SetOpacity(1 - FadeOutTimer.Progress());

      if (FadeOutTimer.Elapsed()) {
        SetOpacity(0);
        FadeOutTimer.Stop();
        OnFadeOut();
      }
    }
  }

  void SetOpacity(float opacity) {
    Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, opacity);
  }
}
