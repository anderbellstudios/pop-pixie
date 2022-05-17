using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologramHoloringAttackAI : AEnemyAI {
  public float InitialDelay;
  public float AttackInterval;
  public int MinAttacks, MaxAttacks;
  public GameObject HoloringPrefab;

  public AEnemyAI WhenFinished;

  IntervalTimer AttackTimer;
  int RemainingAttacks;

  public override void ControlGained() {
    AttackTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = AttackInterval
    };

    RemainingAttacks = Random.Range(MinAttacks, MaxAttacks);

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

    RemainingAttacks--;

    if (RemainingAttacks <= 0) {
      RelinquishControlTo(WhenFinished);
    }
  }
}
