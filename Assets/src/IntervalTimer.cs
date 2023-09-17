using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TimerDelegate();

public class IntervalTimer {

  public float Interval;
  public string TimeClass = "Time";

  public bool Started;

  private float LastElapsed = -1000000f;

  public void Start() {
    Started = true;
  }

  public void Stop() {
    Started = false;
  }

  public bool Elapsed() {
    return Started && TimeSinceElapsed() >= Interval;
  }

  public void Reset() {
    Start();
    LastElapsed = CurrentTime();
  }

  public void IfElapsed(TimerDelegate callback) {
    if (Elapsed()) {
      Reset();
      callback();
    }
  }

  public void UnlessElapsed(TimerDelegate callback) {
    if (Started && !Elapsed()) {
      callback();
    }
  }

  public float TimeSinceElapsed() {
    return CurrentTime() - LastElapsed;
  }

  public float Progress() {
    return Mathf.Clamp(TimeSinceElapsed() / Interval, 0f, 1f);
  }

  float CurrentTime() {
    switch (TimeClass) {
      case "Time":
        return Time.time;

      case "PlayingTime":
        return PlayingTime.time;

      default:
        Debug.Log("Need to add a case branch for " + TimeClass);
        return 0f;
    }
  }

}
