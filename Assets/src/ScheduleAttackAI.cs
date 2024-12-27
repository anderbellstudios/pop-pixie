using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleAttackAI : AMovementEnemyAI {
  [field: SerializeField]
  public override float Speed { get; set; }

  public float MaxDistance = Mathf.Infinity;
  public float AttackInterval, MinDelayBeforeAttack;
  public float RandomnessFactor;
  public bool InterruptWhenOutOfRange = true, InterruptWhenDamaged = true;
  public AMovementEnemyAI NormalAI, AttackAI;

  private AsyncTimer.EnqueuedEvent AttackTimer;
  private bool PreviouslyWithinRange;
  private bool Attacking;
  private float LastAttacked;

  void Start() {
    if (InterruptWhenDamaged) {
      Helper.OnDamage(() => {
        if (AvoidingInterruption)
          return;

        if (Attacking) {
          StopAttack();
        } else {
          // Reset attack timer
          UnscheduleAttack();
          ScheduleAttack();
        }
      });
    }
  }

  protected override void OnActivate() {
    NormalAI.Speed = Speed;

    AttackTimer = null;
    PreviouslyWithinRange = false;
    Attacking = false;
    LastAttacked = -Mathf.Infinity;
  }

  protected override void OnChildFinish(AEnemyAI2 child) {
    if (child == AttackAI) {
      StopAttack();
    } else {
      OnFinish();
    }
  }

  protected override void WhileActive() {
    bool withinRange = WithinRange;

    if (withinRange && !PreviouslyWithinRange) {
      OnEnterRange();
    }

    if (!withinRange && PreviouslyWithinRange) {
      OnExitRange();
    }

    PreviouslyWithinRange = withinRange;
  }

  protected override AMovementEnemyAI UseMovementAI() => Attacking
    ? AttackAI : NormalAI;

  private bool WithinRange => Helper.DistanceToPlayer <= MaxDistance;

  private void OnEnterRange() {
    if (!Attacking && AttackTimer == null) {
      ScheduleAttack();
    }
  }

  private void OnExitRange() {
    if (AttackTimer != null) {
      UnscheduleAttack();
    }

    if (Attacking && InterruptWhenOutOfRange && !AvoidingInterruption) {
      StopAttack();
    }
  }

  private void ScheduleAttack() {
    float timeSinceLastAttack = PlayingTime.time - LastAttacked;

    float randomMultiplier = 1f + Random.Range(-RandomnessFactor, RandomnessFactor);

    float delay = Mathf.Max(
      MinDelayBeforeAttack,
      AttackInterval - timeSinceLastAttack
    ) * randomMultiplier;

    AttackTimer = Helper.SetTimeout(() => {
      AttackTimer = null;
      StartAttack();
    }, delay);
  }

  private void UnscheduleAttack() {
    Helper.ClearTimeout(AttackTimer);
    AttackTimer = null;
  }

  private void StartAttack() {
    Attacking = true;
    LastAttacked = PlayingTime.time;
  }

  private void StopAttack() {
    Attacking = false;

    if (WithinRange) {
      ScheduleAttack();
    }
  }
}
