using System;
using System.Collections;
using System.Collections.Generic;

public delegate void TimerDelegate();

public class IntervalTimer {

  public float Interval;
  public bool Started;

  private DateTime LastElapsed;

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
    LastElapsed = DateTime.Now;
  }

  public void IfElapsed( TimerDelegate callback ) {
    if ( Elapsed() ) {
      Reset();
      callback();
    }
  }

  public void UnlessElapsed( TimerDelegate callback ) {
    if ( Started && !Elapsed() ) {
      callback();
    }
  }

  public float TimeSinceElapsed() {
    return (float) DateTime.Now.Subtract( LastElapsed ).TotalSeconds;
  }

}
