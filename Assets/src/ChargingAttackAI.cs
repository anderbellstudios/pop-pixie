using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingAttackAI : AEnemyAI, IRequiresLineOfMovementAI {
  public float Speed;
  public float Damage;
  public float WiggleAmplitude;
  public float PreparingAttackInterval;
  public float GiveUpInterval;
  public Transform WiggleTransform;

  public SoundController SoundPlayer;
  public List<AudioClip> Sounds;
  public float ChanceToPlaySound = 0f;

  public AEnemyAI WhenAttackFinished;

  IntervalTimer PreparingAttackTimer;
  IntervalTimer GiveUpTimer;

  public override void ControlGained() {
    PreparingAttackTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = PreparingAttackInterval
    };

    GiveUpTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = PreparingAttackInterval + GiveUpInterval
    };

    PreparingAttackTimer.Reset();
    GiveUpTimer.Reset();
  }

  public override void WhileInControl() {
    float randomAngle = Random.Range(-WiggleAmplitude, WiggleAmplitude);
    SetWiggleAngle(randomAngle);

    if (PreparingAttackTimer.Elapsed())
      ApplyMovement(TargetDirection() * Speed);

    if (GiveUpTimer.Elapsed())
      RelinquishControlTo(WhenAttackFinished);
  }

  public override void ControlRelinquished() {
    SetWiggleAngle(0f);
  }

  public override void LocalOnCollisionEnter2D(Collision2D col) {
    if (col.gameObject == Target) {
      PerformAttack();
    }
  }

  void SetWiggleAngle(float angle) {
    (WiggleTransform ?? transform).rotation = Quaternion.Euler(0, 0, angle);
  }

  void PerformAttack() {
    bool isCounterAttack = DamageTarget(Damage, true);

    if (isCounterAttack) {
      HitPoints hp = GetComponent<HitPoints>();
      hp.Damage(Damage * 2);
      if (hp.Dead)
        return;
    } else if (ShouldPlaySound()) {
      // Play attack sound
      int i = Random.Range(0, Sounds.Count);
      var sound = Sounds[i];
      SoundPlayer.Play(sound);
    }

    RelinquishControlTo(WhenAttackFinished);
  }

  bool ShouldPlaySound() {
    return UnityEngine.Random.value < ChanceToPlaySound;
  }
}
