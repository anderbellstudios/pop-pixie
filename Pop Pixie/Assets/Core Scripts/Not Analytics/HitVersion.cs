using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVersion : MonoBehaviour {

  public static bool HitOnAwake = true;
  public NotAnalytics Provider;
  public String Prefix = "";

  void Awake() {
    if (HitOnAwake) {
      Hit();
      HitOnAwake = false;
    }
  }

  public void Hit() {
    Provider.Hit(Prefix + Application.version);
  }

}
