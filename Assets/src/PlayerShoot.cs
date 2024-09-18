using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour {
  public MonoBehaviour AimDirection;
  public FireBullet FireBullet;
  public PlaySound PlaySound;
  public string NoBulletsSoundKey;

  private EquippedWeapon EquippedWeapon;
  private Stopwatch CanFireStopwatch = null;

  void Awake() {
    EquippedWeapon = gameObject.GetComponent<EquippedWeapon>();

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

    PlayerWeapon weapon = EquippedWeapon.CurrentWeapon;
    float cooldown = weapon.CooldownInterval();

    if (WrappedInput.GetButton("Fire") && CanFire(cooldown)) {
      CanFireStopwatch = new Stopwatch.PlayingTime();

      if (weapon.HasBullets()) {
        Fire(weapon);
      } else {
        PlaySound.Play(NoBulletsSoundKey);
      }
    }
  }

  private bool CanFire(float cooldown) =>
    CanFireStopwatch == null || CanFireStopwatch.Time() >= cooldown;

  private void Fire(PlayerWeapon weapon) {
    weapon.ExpendBullet();

    FireBullet.Fire(
      prefab: weapon.BulletPrefab,
      getDirection: () => ScatterDirection.Scatter(
        ((IDirectionManager)AimDirection).Direction,
        amount: weapon.Scatter
      ),
      speed: weapon.BulletSpeed,
      damage: weapon.Damage,
      soundKey: weapon.ShootSoundKey
    );
  }
}
