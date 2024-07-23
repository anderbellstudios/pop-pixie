using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class RingPullCounter : MonoBehaviour {
  public TMP_Text Text;

  public Transform PulseTarget;
  public float PulseDuration;
  public float PulseAmplitude;
  public UnityEvent OnPulse;

  IntervalTimer PulseTimer;

  void Awake() {
    PulseTimer = new IntervalTimer() {
      Interval = PulseDuration
    };
  }

  void Update() {
    Text.text = RingPullsData.Amount().ToString() + " <sprite=\"Ring Pull Icon\" name=\"Ring Pull\">";

    if (RingPullsData.ShouldPulse) {
      RingPullsData.ShouldPulse = false;
      OnPulse.Invoke();
      PulseTimer.Reset();
    }

    if (PulseTimer.Started) {
      float scale = 1 + PulseAmplitude * (1 - PulseTimer.Progress());
      PulseTarget.localScale = new Vector2(scale, scale);
    }
  }
}
