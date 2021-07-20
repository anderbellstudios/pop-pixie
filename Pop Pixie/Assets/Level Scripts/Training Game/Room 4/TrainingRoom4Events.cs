using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainingRoom4Events : MonoBehaviour {
  public TMP_Text TimerText;
  public GameObject HologremsContainer;
  public float DelayAfterRaceFinished;
  public DialogueHopper GoodTimeDialogue, BadTimeDialogue;

  bool RaceInProgress = false;
  bool RaceFinished = false;
  float RaceStartedTime = 0f;
  float RaceFinishedTime = 0f;
  GameObject HologremsContainerClone;

  float ElapsedTime() => (RaceFinished ? RaceFinishedTime : PlayingTime.time) - RaceStartedTime;

  void Awake() {
    HologremsContainer.SetActive(false);
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

  public void HandleStartBoundaryCrossed() {
    if (!RaceInProgress) {
      HologremsContainerClone = Instantiate(HologremsContainer);
      HologremsContainerClone.SetActive(true);

      RaceInProgress = true;
      RaceFinished = false;
      RaceStartedTime = PlayingTime.time;
    }
  }

  public void HandleFinishBoundaryCrossed() {
    if (RaceInProgress) {
      Destroy(HologremsContainerClone);

      RaceInProgress = false;
      RaceFinished = true;
      RaceFinishedTime = PlayingTime.time;

      Invoke("AfterRaceFinished", DelayAfterRaceFinished);
    }
  }

  void AfterRaceFinished() {
    if (ElapsedTime() > 40f) {
      BadTimeDialogue.Hop();
    } else {
      GoodTimeDialogue.Hop();
    }
  }
}
