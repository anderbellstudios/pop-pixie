using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CaptionLineManager : MonoBehaviour {
  public bool SingletonInstance = true;
  public static CaptionLineManager Current;

  public float FadeInDuration, FadeOutDuration;
  public PlaySound PlaySound;
  public TMP_Text BackgroundText, Text;

  private CaptionLine CaptionLine = null;
  private bool Running;
  private Func<float> GetTime;
  private float StartTime;

  void Awake() {
    if (SingletonInstance)
      Current = this;

    SetOpacity(0);
  }

  public void Play(CaptionLine captionLine) {
    CaptionLine = captionLine;

    SetText(CaptionLine.Text);

    if (CaptionLine.HasAudioClip()) {
      PlaySound.Play(CaptionLine.VoiceLineKey, doNotPause: CaptionLine.DoNotPauseWhenNotPlaying);
    }

    GetTime = CaptionLine.DoNotPauseWhenNotPlaying
      ? () => Time.time
      : () => PlayingTime.time;

    Running = true;
    StartTime = GetTime();

    CaptionLine.DialogueMusicFadeBehaviour.ApplyEnterBehaviour();
  }

  void Update() {
    if (!Running)
      return;

    float time = GetTime() - StartTime;

    if (time < FadeInDuration) {
      SetOpacity(time / FadeInDuration);
    } else if (time < CaptionLine.Duration) {
      SetOpacity(1);
    } else {
      SetOpacity(1 - (time - CaptionLine.Duration) / FadeOutDuration);
    }

    if (time >= CaptionLine.Duration + FadeOutDuration) {
      Running = false;
      SetOpacity(0);
      CaptionLine.DialogueMusicFadeBehaviour.ApplyExitBehaviour();
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
