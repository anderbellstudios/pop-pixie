using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainingRoom2Events : MonoBehaviour {
  private bool PastFirstLaser = false;

  void Start() {
    InGamePrompt.Current.RegisterSource(() =>
      PastFirstLaser
      ? null
      : "Press [Roll] while moving to <color=#ffff00>roll</color>"
    );
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      PastFirstLaser = true;
    }
  }
}
