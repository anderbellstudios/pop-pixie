using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RingPullCounter : MonoBehaviour {
  public TMP_Text Text;

  public Transform PulseTarget;
  public float PulseDuration;
  public float PulseAmplitude;
  public UnityEvent OnPulse;

  private int PreviousAmount;
  private Stopwatch PulseStopwatch = null;

  void Start() {
    PreviousAmount = RingPulls();
    UpdateRingPulls();
  }

  void Update() {
    if (RingPulls() != PreviousAmount) {
      UpdateRingPulls();
      PreviousAmount = RingPulls();
    }

    if (RingPullsData.ShouldPulse) {
      RingPullsData.ShouldPulse = false;
      OnPulse.Invoke();
      PulseStopwatch = new Stopwatch.BaseTime();
    }

    if (PulseStopwatch != null) {
      float progress = PulseStopwatch.Progress(PulseDuration);
      float scale = 1f + PulseAmplitude * (1f - progress);
      PulseTarget.localScale = scale * Vector2.one;
    }
  }

  private int RingPulls() => RingPullsData.Amount();

  private void UpdateRingPulls() {
    Text.text = RingPulls().ToString() + " <sprite=\"Ring Pull Icon\" name=\"Ring Pull\">";
  }
}
