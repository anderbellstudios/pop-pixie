using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour {
  public int Flashes = 3;
  public float FlashDuration = 0.2f;
  public Renderer Target;

  private Stopwatch Stopwatch = null;

  public void BeginFlashing() {
    Stopwatch = new Stopwatch.BaseTime();
  }

  void Update() {
    if (Stopwatch == null)
      return;

    float time = Stopwatch.Time();

    Target.enabled = (time / FlashDuration) % 1f < 0.5f;

    if (time > Flashes * FlashDuration) {
      Target.enabled = true;
      Stopwatch = null;
    }
  }
}
