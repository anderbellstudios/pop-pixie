using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interpolator : MonoBehaviour {
  public AnimationCurve AnimationCurve;
  public bool UsePlayingTime = true;
  public UnityEvent<float> OnChange;

  private float Duration;
  private float StartTime;
  private float StartProgress, EndProgress; 
  private bool IsAnimating = false;

  public void Reset(float progress) {
    StartProgress = EndProgress = progress;
    IsAnimating = false;
    HandleChange();
  }

  public void Animate(float startProgress, float endProgress, float duration) {
    StartProgress = startProgress;
    EndProgress = endProgress;
    Duration = duration;
    StartTime = CurrentTime();
    IsAnimating = true;
    HandleChange();
  }

  public void Animate(float endProgress, float duration) {
    Animate(CurrentProgress(), endProgress, duration);
  }

  public float Evaluate() {
    return AnimationCurve.Evaluate(CurrentProgress());
  }

  void Update() {
    if (IsAnimating) HandleChange();
  }

  private void HandleChange() {
    OnChange.Invoke(Evaluate());
  }

  private float CurrentTime() {
    return UsePlayingTime ? PlayingTime.time : Time.time;
  }

  private float CurrentProgress() {
    if (IsAnimating) {
      float time = Mathf.Clamp01((CurrentTime() - StartTime) / Duration);

      if (time >= 1) {
        IsAnimating = false;
        time = 1;
      }

      return Mathf.Lerp(StartProgress, EndProgress, time);
    } else {
      return EndProgress;
    }
  }
}
