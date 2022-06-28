using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour {
  public bool PerformOnStart;
  public Image Image;
  public float Duration;
  public AnimationCurve AnimationCurve;

  private IntervalTimer Timer;

  void Start() {
    Timer = new IntervalTimer() {
      Interval = Duration
    };

    if (PerformOnStart)
      Perform();
  }

  public void Perform() {
    Timer.Reset();
  }

  void Update() {
    if (Timer.Started) {
      Image.color = new Color(1, 1, 1, AnimationCurve.Evaluate(Timer.Progress()));
      Timer.IfElapsed(() => Timer.Stop());
    }
  }
}
