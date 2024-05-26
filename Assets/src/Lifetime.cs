using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour {
  public float Interval;
  public bool UsePlayingTime = true;

  void Start() {
    AsyncTimer timer = UsePlayingTime ? AsyncTimer.PlayingTime : AsyncTimer.BaseTime;
    timer.SetTimeout(() => Destroy(gameObject), Interval);
  }
}
