using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingRoom2Events : MonoBehaviour {
  public GameObject RingPull;

  private bool PastFirstLaser = false;
  private bool RingPullCollected = false;

  void Start() {
    InGamePrompt.Current.RegisterSource(() =>
      PastFirstLaser
      ? null
      : "Press <color=#00ffff>[Roll]</color> while moving to avoid taking damage"
    );
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      PastFirstLaser = true;
    }
  }

  void Update() {
    if (!RingPullCollected && RingPull == null) {
      RingPullCollected = true;
      Invoke("AfterRingPullCollected", 2.0f);
    }
  }

  void AfterRingPullCollected() {
    Debug.Log("AfterRingPullCollected");
    SceneManager.LoadScene("Training Room 3");
  }
}
