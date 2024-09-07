using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInTestMode : MonoBehaviour {
  public static bool TestMode;

  public List<Behaviour> Behaviours;

  void Awake() {
    if (TestMode) {
      Behaviours.ForEach(behaviour => {
        behaviour.enabled = false;
      });
    }
  }
}
