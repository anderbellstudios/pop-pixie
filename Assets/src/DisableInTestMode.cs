using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInTestMode : MonoBehaviour {
  public List<Behaviour> Behaviours;

  void Awake() {
    if (TestMode.Enabled) {
      Behaviours.ForEach(behaviour => {
        behaviour.enabled = false;
      });
    }
  }
}
