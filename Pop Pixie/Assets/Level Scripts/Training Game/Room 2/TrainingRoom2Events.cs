using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingRoom2Events : MonoBehaviour {
  public GameObject RingPull;

  private bool RingPullCollected = false;

  void Update() {
    if (!RingPullCollected && RingPull == null) {
      RingPullCollected = true;
      Invoke("AfterRingPullCollected", 2.0f);
    }
  }

  void AfterRingPullCollected() {
    Debug.Log("AfterRingPullCollected");
    SceneManager.LoadScene("Training Room 1");
  }
}
