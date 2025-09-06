using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimulationHitsTakenCounter : MonoBehaviour {
  public TMP_Text ValueText;

  public Color GoodColor,
    BadColor;
  public int GoodHitsTaken;

  private LowPriorityBehaviour LowPriorityBehaviour;

  void Awake() {
    LowPriorityBehaviour = new LowPriorityBehaviour();
  }

  void Update() {
    LowPriorityBehaviour.EveryNFrames(
      10,
      () => {
        int hitsTaken = SimulationResultData.NumberOfHitsTaken;
        bool goodHitsTaken = hitsTaken <= GoodHitsTaken;

        ValueText.text = hitsTaken.ToString();
        ValueText.color = hitsTaken <= GoodHitsTaken ? GoodColor : BadColor;
      }
    );
  }
}
