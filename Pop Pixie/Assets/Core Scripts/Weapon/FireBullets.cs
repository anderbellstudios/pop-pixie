using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireBullets : MonoBehaviour {
  public FireBullet FireBullet;
  public float Duration = -1;
  public float BulletsPerSecond = 1;
  public GameObject BulletPrefab;
  public float BulletSpeed;
  public float BulletDamage;
  public float BulletLifetime = 100;
  public AudioClip FireSound;
  public UnityEvent OnStopFiring;

  Func<Vector3> GetDirection;
  bool Firing;
  float BeganFiringAt;
  int BulletsFired;

  public void BeginFiring(Func<Vector3> getDirection) {
    GetDirection = getDirection;
    Firing = true;
    BeganFiringAt = PlayingTime.time;
    BulletsFired = 0;
  }

  public void StopFiring() {
    Firing = false;
    OnStopFiring.Invoke();
  }


  void Update() {
    if (!Firing || !StateManager.Playing)
      return;

    float timeSinceBegan = PlayingTime.time - BeganFiringAt;

    if (Duration > 0 && timeSinceBegan > Duration) {
      StopFiring();
      return;
    }

    int expectedBulletsFired = (int) Mathf.Floor(timeSinceBegan * BulletsPerSecond);

    if (expectedBulletsFired > BulletsFired) {
      Fire();
      BulletsFired++;
    }
  }

  void Fire() {
    FireBullet.Fire(
      prefab: BulletPrefab,
      getDirection: GetDirection,
      speed: BulletSpeed,
      damage: BulletDamage,
      lifetime: BulletLifetime,
      sound: FireSound
    );
  }
}
