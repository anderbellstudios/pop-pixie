using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour {
  public float Interval;

  void Start() {
    AsyncTimer.PlayingTime.SetTimeout(() => {
      Destroy(gameObject);
    }, Interval);
  }
}
