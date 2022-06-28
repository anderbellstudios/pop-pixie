using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Captions : MonoBehaviour {

  public float Duration;
  public TMP_Text Label;

  private IntervalTimer DisappearTimer;

  void Awake() {
    DisappearTimer = new IntervalTimer() {
      Interval = Duration
    };
  }

  void Update() {
    DisappearTimer.IfElapsed( delegate {
      DisappearTimer.Stop();
      ClearText();
    });
  }

  public void SetText(string text) {
    Label.text = text;
    DisappearTimer.Reset();
  }

  public void ClearText() {
    Label.text = "";
  }

}
