using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleGameObject : MonoBehaviour {
  public bool PerformOnStart;
  public Transform Transform;
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
      float scale = AnimationCurve.Evaluate(Timer.Progress());
      Transform.localScale = new Vector3(scale, scale, scale);
      Timer.IfElapsed(() => Timer.Stop());
    }
  }
}
