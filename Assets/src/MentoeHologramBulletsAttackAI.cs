using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologramBulletsAttackAI : AEnemyAI {
  public FireBullet FireBullet;

  public float Rotations;
  public float Duration;
  public GameObject BulletPrefab;
  public float BulletsPerSecond;
  public float BulletSpeed;
  public float BulletDamage;
  public string ShootSoundKey;

  public AEnemyAI WhenFinished;

  private Stopwatch AngleStopwatch;
  private Vector3 ReferenceDirection;
  private float CurrentRotations;

  void Start() {
    PreloadProgrammerSounds.PreloadSound(ShootSoundKey);
  }

  public override void ControlGained() {
    ReferenceDirection = TargetDirection();
    CurrentRotations = Rotations;

    AngleStopwatch = new Stopwatch.PlayingTime();

    SetTimeout(() => {
      RelinquishControlTo(WhenFinished);
    }, Duration);

    SetInterval(FireBullets, 1f / BulletsPerSecond);
  }

  private void FireBullets() {
    float progress = AngleStopwatch.Progress(Duration);

    // Add 15deg to the angle to avoid shooting the player right away
    float angle = Mathf.Lerp(0, 360 * CurrentRotations, progress) + 15;

    FireBulletInDirection(Quaternion.Euler(0, 0, angle + 0) * Vector3.right, true);
    FireBulletInDirection(Quaternion.Euler(0, 0, angle + 90) * Vector3.right, false);
    FireBulletInDirection(Quaternion.Euler(0, 0, angle + 180) * Vector3.right, false);
    FireBulletInDirection(Quaternion.Euler(0, 0, angle + 270) * Vector3.right, false);
  }

  private void FireBulletInDirection(Vector3 direction, bool playFireSound) {
    FireBullet.Fire(
      prefab: BulletPrefab,
      getDirection: () => direction.normalized,
      speed: BulletSpeed,
      damage: BulletDamage,
      soundKey: playFireSound ? ShootSoundKey : ""
    );
  }
}
