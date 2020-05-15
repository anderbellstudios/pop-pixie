using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVersion : MonoBehaviour {

  public bool HitOnAwake = true;
  public NotAnalytics Provider;

  void Awake() {
    if (HitOnAwake)
      Hit();
  }

  public void Hit() {
    Provider.Hit(Application.version);
  }

}
