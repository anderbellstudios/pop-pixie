using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologramBulletsAttackAI : AEnemyAI {
  public FireBullet FireBullet;

  public float Duration;
  public GameObject BulletPrefab;
  public float BulletsPerSecond;
  public float BulletSpeed;
  public float BulletDamage;
  public AudioClip FireSound;

  public AEnemyAI WhenFinished;

  private IntervalTimer AngleTimer, FireTimer;
  private Vector3 ReferenceDirection;

  public override void ControlGained() {
    AngleTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = Duration
    };

    AngleTimer.Reset();

    FireTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = 1f / BulletsPerSecond
    };

    FireTimer.Start();

    ReferenceDirection = TargetDirection();
  }

  public override void WhileInControl() {
    if (AngleTimer.Elapsed()) {
      RelinquishControlTo(WhenFinished);
    } else {
      FireTimer.IfElapsed(() => {
        // Add 15deg to the angle to avoid shooting the player right away
        float angle = Mathf.Lerp(0, 360, AngleTimer.Progress()) + 15;

        FireBulletInDirection(Quaternion.Euler(0, 0, angle +   0) * Vector3.right);
        FireBulletInDirection(Quaternion.Euler(0, 0, angle +  90) * Vector3.right);
        FireBulletInDirection(Quaternion.Euler(0, 0, angle + 180) * Vector3.right);
        FireBulletInDirection(Quaternion.Euler(0, 0, angle + 270) * Vector3.right);
      });
    }
  }

  void FireBulletInDirection(Vector3 direction) {
    FireBullet.Fire(
      prefab: BulletPrefab,
      getDirection: () => direction.normalized,
      speed: BulletSpeed,
      damage: BulletDamage,
      sound: FireSound
    );
  }
}
