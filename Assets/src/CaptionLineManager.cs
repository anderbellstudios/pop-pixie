using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CaptionLineManager : MonoBehaviour {
  public bool SingletonInstance = true;
  public static CaptionLineManager Current;

  public float FadeInDuration, FadeOutDuration;
  public SoundController SoundController;
  public TMP_Text BackgroundText, Text;

  private CaptionLine CaptionLine = null;
  private IntervalTimer Timer;

  void Awake () {
    if (SingletonInstance)
      Current = this;

    Timer = new IntervalTimer();

    SetOpacity(0);
  }

  public void Play(CaptionLine captionLine) {
    CaptionLine = captionLine;

    SetText(CaptionLine.Text);

    if (CaptionLine.AudioClip != null)
      SoundController.Play(CaptionLine.AudioClip);

    Timer.Interval = captionLine.Duration + FadeOutDuration;
    Timer.Reset();
  }

  void Update() {
    if (Timer.Started) {
      float time = Timer.TimeSinceElapsed();

      if (time < FadeInDuration) {
        SetOpacity(time / FadeInDuration);
      } else if (time < CaptionLine.Duration) {
        SetOpacity(1);
      } else {
        SetOpacity(1 - (time - CaptionLine.Duration) / FadeOutDuration);
      }

      Timer.IfElapsed(() => {
        Timer.Stop();
        SetOpacity(0);
      });
    }
  }

  void SetText(string text) {
    Text.text = text;
    BackgroundText.text = String.Format("<mark=#000000 padding=\"20,20,10,10\">{0}</mark>", text);
  }

  void SetOpacity(float opacity) {
    BackgroundText.color = Text.color = new Color(1, 1, 1, opacity);
  }
}
