using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingGamePlayerHitPointEvents : MonoBehaviour {
  private Stopwatch SlowdownStopwatch = null;

  void Awake() {
    HitPoints hp = GetComponent<HitPoints>();
    MovementManager movementManager = GetComponent<MovementManager>();

    hp.OnDecrease.AddListener(hp => {
      SimulationResultData.NumberOfHitsTaken++;

      if (SlowdownStopwatch != null)
        return;

      SlowdownStopwatch = new Stopwatch.BaseTime();

      ScreenFade.DamageFlash();
    });

    movementManager.SpeedModifiers.Add(
      (s) => {
        if (SlowdownStopwatch == null)
          return s;

        float progress = SlowdownStopwatch.Progress(2f);
        float k = (1f - Mathf.Cos(progress * Mathf.PI)) / 2f;

        if (progress >= 1f) {
          SlowdownStopwatch = null;
        }

        return s * k;
      }
    );
  }
}
