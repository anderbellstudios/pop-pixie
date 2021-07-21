using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologram : AEnemyAI {
  public float InitialDelay;
  public float AttackInterval;
  public GameObject HoloringPrefab;

  IntervalTimer AttackTimer;

  public override void ControlGained() {
    AttackTimer = new IntervalTimer() {
      Interval = AttackInterval,
      TimeClass = "PlayingTime"
    };

    Invoke("StartTimer", InitialDelay);
  }

  void StartTimer() {
    AttackTimer.Start();
  }

  public override void WhileInControl() {
    AttackTimer.IfElapsed(() => PerformAttack());
  }

  void PerformAttack() {
    Instantiate(HoloringPrefab, transform);
  }

  public void HandleCollidedWithPlayer() {
    Target.GetComponent<HitPoints>().Damage(1);
  }
}
