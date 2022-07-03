using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour {
  public float Interval;
  private IntervalTimer DespawnTimer;

  void Start() {
    DespawnTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = Interval
    };

    DespawnTimer.Reset();
  }

  void Update() {
    DespawnTimer.IfElapsed(() => Destroy(gameObject));
  }
}
