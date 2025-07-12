using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stopwatch {
  protected abstract float CurrentTime();

  public class BaseTime : Stopwatch {
    protected override float CurrentTime() => UnityEngine.Time.time;
  }

  public class PlayingTime : Stopwatch {
    protected override float CurrentTime() => global::PlayingTime.time;
  }

  private float StartTime;

  public Stopwatch() {
    Reset();
  }

  public void Reset() {
    StartTime = CurrentTime();
  }

  public float Time() => CurrentTime() - StartTime;

  public float UnclampedProgress(float duration) => Time() / duration;

  public float LoopedProgress(float duration) => UnclampedProgress(duration) % 1f;

  public float Progress(float duration) => Mathf.Clamp01(UnclampedProgress(duration));
}
