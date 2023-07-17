using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainingRoom4Events : MonoBehaviour {
  public TMP_Text TimerText;
  public GameObject HologremsContainer;
  public SoundHopper StartedFinishedSound;
  public float StartedFinishedSoundDuration;
  public SongHopper RaceSong;
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

  void Start() {
    InvokeRepeating("UpdateTimer", 0f, 0.25f);
  }

  void UpdateTimer() {
    if (RaceInProgress || RaceFinished) {
      float elapsedTime = ElapsedTime();
      int minutes = (int) Mathf.Floor(Mathf.Clamp(elapsedTime / 60f, 0, 99));
      int seconds = (int) Mathf.Floor(elapsedTime - (minutes * 60));

      TimerText.text = String.Format("{0:00}:{1:00}", minutes, seconds);
    } else {
      TimerText.text = "00:00";
    }
  }

  // Start race
  public void HandleStartBoundaryCrossed() {
    if (!RaceInProgress) {
      HologremsContainerClone = Instantiate(HologremsContainer);
      HologremsContainerClone.SetActive(true);

      RaceInProgress = true;
      RaceFinished = false;
      RaceStartedTime = PlayingTime.time;
      StartedFinishedSound.Hop();
      AsyncTimer.BaseTime.SetTimeout(() => RaceSong.Hop(), StartedFinishedSoundDuration);
    }
  }

  // Finish race
  public void HandleFinishBoundaryCrossed() {
    if (RaceInProgress) {
      Destroy(HologremsContainerClone);

      RaceInProgress = false;
      RaceFinished = true;
      RaceFinishedTime = PlayingTime.time;
      StartedFinishedSound.Hop();
      AsyncTimer.BaseTime.SetTimeout(() => RaceSong.Stop(), StartedFinishedSoundDuration);

      if (!SimulationResultData.ObstacleCourseBestTime.HasValue || ElapsedTime() < SimulationResultData.ObstacleCourseBestTime)
        SimulationResultData.ObstacleCourseBestTime = (int) ElapsedTime();

      AsyncTimer.PlayingTime.SetTimeout(AfterRaceFinished, DelayAfterRaceFinished);
    }
  }

  void AfterRaceFinished() {
    // Timer rounds down, so anything less than 41 seconds is a good time
    bool goodTime = ElapsedTime() < 41f;
    NotAnalytics.Current.Hit("finished-obstacle-course", goodTime ? "good-time" : "bad-time");
    (goodTime ? GoodTimeDialogue : BadTimeDialogue).Hop();
  }
}
