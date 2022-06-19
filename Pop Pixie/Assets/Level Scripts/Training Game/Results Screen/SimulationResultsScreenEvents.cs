using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimulationResultsScreenEvents : MonoBehaviour {
  public TMP_Text CompletionTimeText, NumberOfHitsTakenText, ObstacleCourseBestTimeText;
  public WaitPhase WaitForConfirm;

  public Color GoodColor, BadColor;
  public int GoodCompletionTime, GoodHitsTaken, GoodBestTime;
  public DialogueHopper GoodDialogue, BadDialogue;

  void Awake() {
    SimulationResultData.FinishedTime = PlayingTime.time;

    float completionTime = SimulationResultData.CompletionTime;
    int hitsTaken = SimulationResultData.NumberOfHitsTaken;
    int? bestTime = SimulationResultData.ObstacleCourseBestTime;

    bool goodCompletionTime = completionTime <= GoodCompletionTime;
    bool goodHitsTaken = hitsTaken <= GoodHitsTaken;
    bool goodBestTime = bestTime <= GoodBestTime;

    CompletionTimeText.text = TimeSpan.FromSeconds(completionTime).ToString(@"hh\:mm\:ss");
    CompletionTimeText.color = completionTime <= GoodCompletionTime ? GoodColor : BadColor;
    NotAnalytics.Current.Hit("training-simulation-results:completion-time", goodCompletionTime ? "good" : "bad");

    NumberOfHitsTakenText.text = hitsTaken.ToString();
    NumberOfHitsTakenText.color = hitsTaken <= GoodHitsTaken ? GoodColor : BadColor;
    NotAnalytics.Current.Hit("training-simulation-results:hits-taken", goodHitsTaken ? "good" : "bad");

    ObstacleCourseBestTimeText.text = String.Format("{0}s", bestTime ?? 40);
    ObstacleCourseBestTimeText.color = bestTime <= GoodBestTime ? GoodColor : BadColor;
    NotAnalytics.Current.Hit("training-simulation-results:obstacle-course-best-time", goodBestTime ? "good" : "bad");
  }

  public void PlayResultsDialogue() {
    (SimulationResultData.NumberOfHitsTaken <= GoodHitsTaken ? GoodDialogue : BadDialogue).Hop();
  }

  void Update() {
    if (WrappedInput.GetButtonDown("Confirm"))
      WaitForConfirm.StopWaiting();
  }
}
