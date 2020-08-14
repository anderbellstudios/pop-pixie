using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistMode : MonoBehaviour {

  public HitPoints HitPoints;

  void Awake() {
    HitPoints.Maximum *= (int) AssistModeData.HPAdjustment;
  }

}
