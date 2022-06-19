using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotAnalyticsHitHopper : MonoBehaviour {
  public string Event;
  public bool HopOnAwake = false;

  void Awake() {
    if (HopOnAwake)
      Hop();
  }

  public void Hop() {
    NotAnalytics.Current.Hit(Event);
  }
}
