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
  private Queue<CaptionLine> CaptionLineQueue = new();
  private bool Running;
  private float StartTime;
  private float CurrentTime;

  void Awake() {
    if (SingletonInstance)
      Current = this;

    SetOpacity(0);
  }

  public void Play(CaptionLine captionLine) {
    if (Running) {
      if (captionLine != CaptionLine && !CaptionLineQueue.Contains(captionLine)) {
        CaptionLineQueue.Enqueue(captionLine);
      }
      return;
    }

    CaptionLine = captionLine;

    SetText(CaptionLine.Text);

    if (CaptionLine.HasAudioClip()) {
      PlaySound.Play(CaptionLine.VoiceLineKey, doNotPause: CaptionLine.IgnorePause);
    }

    Running = true;
    StartTime = CurrentTime;

    CaptionLine.DialogueMusicFadeBehaviour.ApplyEnterBehaviour();
  }

  void Update() {
    if (!Running)
      return;

    UpdateTime();

    float time = CurrentTime - StartTime;

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
      PlayEnqueued();
    }
  }

  void UpdateTime() {
    if (
      Running && (
        CaptionLine.IgnorePause ||
        !StateManager.Enabled(StateFeatures.PauseSounds)
      )
    ) {
      CurrentTime += Time.deltaTime;
    }
  }

  void SetText(string text) {
    Text.text = text;
    BackgroundText.text = String.Format("<mark=#000000 padding=\"20,20,10,10\">{0}</mark>", text);
  }

  void SetOpacity(float opacity) {
    BackgroundText.color = Text.color = new Color(1, 1, 1, opacity);
  }

  void PlayEnqueued() {
    if (CaptionLineQueue.Count > 0) {
      Play(CaptionLineQueue.Dequeue());
    }
  }
}
