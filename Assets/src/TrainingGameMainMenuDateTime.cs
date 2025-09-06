using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainingGameMainMenuDateTime : MonoBehaviour {
  public TMP_Text Text;

  void Start() {
    InvokeRepeating("UpdateTime", 0f, 1f);
  }

  private void UpdateTime() {
    Text.text = Time();
  }

  private string Time() {
    if (TestMode.Enabled)
      return "Thursday 20 February\n12:00:00";

    return DateTime.Now.ToString("dddd d MMMM\nHH:mm:ss");
  }
}
