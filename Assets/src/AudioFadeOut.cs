using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeOut : MonoBehaviour {
  public bool SingletonInstance = true;
  public static AudioFadeOut Current;
  public float DialogueMusicFadeDuration = 0.5f;
  public float DialogueMusicFadedVolume = 0.25f;

  private IntervalTimer FadeOutTimer;
  private bool IgnoreMusic = false;

  private bool DialogueMusicFade = false;
  private bool DialogueMusicFadeChanging = false;
  private float DialogueMusicFadeProgress = 0;

  void Awake() {
    if (SingletonInstance)
      Current = this;

    FadeOutTimer = new IntervalTimer();
  }

  void Update() {
    if (DialogueMusicFadeChanging) {
      DialogueMusicFadeProgress = Mathf.Clamp01(DialogueMusicFadeProgress + (DialogueMusicFade ? 1 : -1) * Time.deltaTime / DialogueMusicFadeDuration);

      if (DialogueMusicFadeProgress == 0 || DialogueMusicFadeProgress == 1) {
        DialogueMusicFadeChanging = false;
      }
    }
  }

  public void FadeOut(float duration, bool ignoreMusic = true) {
    FadeOutTimer.Interval = duration;
    FadeOutTimer.Reset();
    IgnoreMusic = ignoreMusic;
  }

  public void SetDialogueMusicFade(bool dialogueMusicFade) {
    DialogueMusicFade = dialogueMusicFade;
    DialogueMusicFadeChanging = true;
  }

  public float FadeLevel(bool isMusic = false) {
    float baseFadeLevel = FadeOutTimer.Started && (!isMusic || !IgnoreMusic)
      ? 1f - FadeOutTimer.Progress()
      : 1f;

    float dialogueMusicFadeLevel = isMusic
      ? Mathf.Lerp(1, DialogueMusicFadedVolume, DialogueMusicFadeProgress)
      : 1f;

    return baseFadeLevel * dialogueMusicFadeLevel;
  }
}
