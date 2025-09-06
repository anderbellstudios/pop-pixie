using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour {
  public MonoBehaviour AimDirection;
  public EquippedWeapon EquippedWeapon;
  public Crouch Crouch;
  public FireBullet FireBullet;
  public PlaySound PlaySound;
  public string NoBulletsSoundKey;

  private Stopwatch CanFireStopwatch = null;
  private Stopwatch NoBulletsSoundStopwatch = null;

  void Awake() {
    EquippedWeapon.OnChangeWeapon.AddListener(() => {
      CanFireStopwatch = null;
    });
  }

  void Start() {
    PreloadProgrammerSounds.PreloadSound(NoBulletsSoundKey);
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    if (WrappedInput.GetButton("Fire")) {
      TryToFire();
    }
  }

  private void TryToFire() {
    PlayerWeapon weapon = EquippedWeapon.CurrentWeapon;
    float cooldown = weapon.CooldownInterval();

    if (!CanFire(cooldown))
      return;

    if (weapon.HasBullets()) {
      Fire(weapon);
    } else if (ShouldPlayNoBulletsSound(cooldown)) {
      PlayNoBulletsSound();
    }
  }

  private bool CanFire(float cooldown) =>
    !Crouch.InCrouchZone && (CanFireStopwatch == null || CanFireStopwatch.Time() >= cooldown);

  private bool ShouldPlayNoBulletsSound(float cooldown) =>
    NoBulletsSoundStopwatch == null || NoBulletsSoundStopwatch.Time() >= cooldown;

  private void Fire(PlayerWeapon weapon) {
    CanFireStopwatch = new Stopwatch.PlayingTime();
    weapon.ExpendBullet();

    FireBullet.Fire(
      prefab: weapon.BulletPrefab,
      getDirection: () =>
        ScatterDirection.Scatter(
          ((IDirectionManager)AimDirection).Direction,
          amount: weapon.Scatter
        ),
      speed: weapon.BulletSpeed,
      damage: weapon.Damage,
      soundKey: weapon.ShootSoundKey
    );
  }

  private void PlayNoBulletsSound() {
    NoBulletsSoundStopwatch = new Stopwatch.PlayingTime();
    PlaySound.Play(NoBulletsSoundKey);
  }
}
