using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingAttackAI : ALegacyEnemyAI, IRequiresLineOfMovementAI {
  public float Speed;
  public float Damage;
  public float WiggleAmplitude;
  public float PreparingAttackInterval;
  public float GiveUpInterval;
  public Transform WiggleTransform;

  // public SoundController SoundPlayer;
  public List<AudioClip> Sounds;
  public float ChanceToPlaySound = 0f;

  public ALegacyEnemyAI WhenAttackFinished;

  private bool Preparing;

  public override void ControlGained() {
    Preparing = true;

    SetTimeout(() => {
      Preparing = false;

      SetTimeout(() => {
        RelinquishControlTo(WhenAttackFinished);
      }, GiveUpInterval);
    }, PreparingAttackInterval);
  }

  public override void WhileInControl() {
    float randomAngle = Random.Range(-WiggleAmplitude, WiggleAmplitude);
    SetWiggleAngle(randomAngle);

    if (!Preparing)
      ApplyMovement(TargetDirection() * Speed);
  }

  public override void ControlRelinquished() {
    SetWiggleAngle(0f);
  }

  public override void LocalOnCollisionEnter2D(Collision2D col) {
    if (col.gameObject == Target) {
      PerformAttack();
    }
  }

  private void SetWiggleAngle(float angle) {
    (WiggleTransform ?? transform).rotation = Quaternion.Euler(0, 0, angle);
  }

  private void PerformAttack() {
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
      Debug.LogError("ChargingAttackAI is using deprecated SoundController");
      // SoundPlayer.Play(sound);
    }

    RelinquishControlTo(WhenAttackFinished);
  }

  private bool ShouldPlaySound() => Random.value < ChanceToPlaySound;
}
