using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RollPastLaserHint : MonoBehaviour {
  private bool PastLaser = false;

  void Start() {
    InGamePrompt.Current.RegisterSource(100, () =>
      PastLaser
      ? null
      : "Press [Roll] while moving to <color=#ffff00>roll</color>"
    );
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      PastLaser = true;
    }
  }
}
