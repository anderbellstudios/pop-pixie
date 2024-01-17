using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingGamePlayerHitPointEvents : MonoBehaviour {
  private bool Active = false;
  private float ActivatedAt;

  void Awake() {
    HitPoints hp = GetComponent<HitPoints>();
    MovementManager movementManager = GetComponent<MovementManager>();

    hp.OnDecrease.AddListener(hp => {
      SimulationResultData.NumberOfHitsTaken++;

      if (Active)
        return;
      Active = true;
      ActivatedAt = Time.time;

      ScreenFade.Current.Flash("red", 2f);

      AsyncTimer.BaseTime.SetTimeout(() => {
        Active = false;
      }, 2f);
    });

    movementManager.SpeedModifiers.Add((s) => {
      if (!Active)
        return s;
      float progress = (Time.time - ActivatedAt) / 2;
      float k = (1 - Mathf.Cos(progress * Mathf.PI)) / 2;
      return s * k;
    });
  }
}
