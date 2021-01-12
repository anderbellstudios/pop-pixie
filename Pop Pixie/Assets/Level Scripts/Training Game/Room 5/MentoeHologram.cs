using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologram : AEnemyAI {
  public float AttackInterval;
  public GameObject HoloringPrefab;

  IntervalTimer AttackTimer;

  public override void ControlGained() {
    AttackTimer = new IntervalTimer() {
      Interval = AttackInterval,
      TimeClass = "PlayingTime"
    };

    AttackTimer.Reset();
  }

  public override void WhileInControl() {
    AttackTimer.IfElapsed(() => PerformAttack());
  }

  void PerformAttack() {
    Instantiate(HoloringPrefab, transform);
  }

  public void HandleCollidedWithPlayer() {
    GameObject.FindGameObjectWithTag("Player").GetComponent<HitPoints>().Damage(1);
  }
}
