using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour {
  public bool PerformOnStart;
  public Image Image;
  public float Duration;
  public AnimationCurve AnimationCurve;

  private Stopwatch Stopwatch;

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
    Image.color = new Color(1, 1, 1, AnimationCurve.Evaluate(progress));

    if (progress >= 1f) {
      Stopwatch = null;
    }
  }
}
