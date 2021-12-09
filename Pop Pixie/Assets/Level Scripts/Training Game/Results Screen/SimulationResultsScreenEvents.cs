using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimulationResultsScreenEvents : MonoBehaviour {
  public TMP_Text CompletionTimeText, NumberOfHitsTakenText, ObstacleCourseBestTimeText;

  public Color GoodColor, BadColor;
  public int GoodCompletionTime, GoodHitsTaken, GoodBestTime;
  public float DelayAfterRevealingResults;
  public DialogueHopper GoodDialogue, BadDialogue;

  void Awake() {
    SimulationResultData.FinishedTime = DateTime.Now;
  }

  public void RevealResults() {
    TimeSpan? completionTime = SimulationResultData.CompletionTime;
    int hitsTaken = SimulationResultData.NumberOfHitsTaken;
    int? bestTime = SimulationResultData.ObstacleCourseBestTime;

    CompletionTimeText.text = completionTime?.ToString(@"hh\:mm\:ss") ?? "--:--:--";
    CompletionTimeText.color = (completionTime?.TotalSeconds ?? 0) <= GoodCompletionTime ? GoodColor : BadColor;

    NumberOfHitsTakenText.text = hitsTaken.ToString();
    NumberOfHitsTakenText.color = hitsTaken <= GoodHitsTaken ? GoodColor : BadColor;

    ObstacleCourseBestTimeText.text = String.Format("{0}s", bestTime ?? 40);
    ObstacleCourseBestTimeText.color = bestTime <= GoodBestTime ? GoodColor : BadColor;

    Invoke("PlayResultsDialogue", DelayAfterRevealingResults);
  }

  void PlayResultsDialogue() {
    (SimulationResultData.NumberOfHitsTaken <= GoodHitsTaken ? GoodDialogue : BadDialogue).Hop();
  }
}
