using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleGameObject : MonoBehaviour {
  public bool PerformOnStart;
  public Transform Transform;
  public float Duration;
  public AnimationCurve AnimationCurve;

  private Stopwatch Stopwatch = null;

  void Start() {
    if (PerformOnStart)
      Perform();
  }

  public void Perform() {
    Stopwatch = new Stopwatch.BaseTime();
  }

  void Update() {
    if (Stopwatch == null)
      return;

    float progress = Stopwatch.Progress(Duration);
    float scale = AnimationCurve.Evaluate(progress);
    Transform.localScale = scale * Vector3.one;

    if (progress >= 1f) {
      Stopwatch = null;
    }
  }
}
