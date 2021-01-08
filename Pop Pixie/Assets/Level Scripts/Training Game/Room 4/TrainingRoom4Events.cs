using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using UnityEngine.SceneManagement;

public class TrainingRoom4Events : MonoBehaviour {
  public TMP_Text TimerText;

  bool RaceInProgress = false;
  bool RaceFinished = false;
  float RaceStartedTime = 0f;
  float RaceFinishedTime = 0f;

  float ElapsedTime() => (RaceFinished ? RaceFinishedTime : PlayingTime.time) - RaceStartedTime;

  public void HandleStartBoundaryCrossed() {
    if (!RaceInProgress) {
      RaceInProgress = true;
      RaceFinished = false;
      RaceStartedTime = PlayingTime.time;
    }
  }

  public void HandleFinishBoundaryCrossed() {
    if (RaceInProgress) {
      RaceInProgress = false;
      RaceFinished = true;
      RaceFinishedTime = PlayingTime.time;
    }
  }

  void Update() {
    if (RaceInProgress || RaceFinished) {
      float elapsedTime = ElapsedTime();
      int minutes = (int) Mathf.Floor(Mathf.Clamp(elapsedTime / 60f, 0, 99));
      int seconds = (int) Mathf.Floor(elapsedTime - (minutes * 60));

      TimerText.text = String.Format("{0:00}:{1:00}", minutes, seconds);
    } else {
      TimerText.text = "00:00";
    }
  }
}
