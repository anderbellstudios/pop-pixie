using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitSceneStarted : MonoBehaviour {
  void Start() {
    NotAnalytics.Current.Hit("scene-started", SceneManager.GetActiveScene().name);
  }
}
