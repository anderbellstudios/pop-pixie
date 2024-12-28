using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireBullets : MonoBehaviour {
  public FireBullet FireBullet;
  public float BulletsPerSecond = 1;
  public GameObject BulletPrefab;
  public float BulletSpeed;
  public float BulletDamage;
  public float CounterAttackDamage;
  public string ShootSoundKey;

  public bool Firing { get; private set; }

  private Func<Vector3> GetDirection, GetTarget, GetOrigin;
  private int BulletsFired;
  private Stopwatch Stopwatch;

  void Start() {
    PreloadProgrammerSounds.PreloadSound(ShootSoundKey);
  }

  public void BeginFiring(
    Func<Vector3> getDirection = null,
    Func<Vector3> getTarget = null,
    Func<Vector3> getOrigin = null
  ) {
    GetDirection = getDirection;
    GetTarget = getTarget;
    GetOrigin = getOrigin;
    Firing = true;
    BulletsFired = 0;
    Stopwatch = new Stopwatch.PlayingTime();
  }

  public void StopFiring() {
    Firing = false;
  }


  void Update() {
    if (!Firing || !StateManager.Playing)
      return;

    float timeSinceBegan = Stopwatch.Time();
    int expectedBulletsFired = (int)Mathf.Floor(timeSinceBegan * BulletsPerSecond);

    if (expectedBulletsFired > BulletsFired) {
      Fire();
      BulletsFired++;
    }
  }

  void Fire() {
    FireBullet.Fire(
      prefab: BulletPrefab,
      getDirection: GetDirection,
      getTarget: GetTarget,
      origin: GetOrigin == null ? null : GetOrigin(),
      speed: BulletSpeed,
      damage: BulletDamage,
      counterAttackDamage: CounterAttackDamage,
      soundKey: ShootSoundKey
    );
  }
}
