using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour {
  public MonoBehaviour AimDirection;
  public FireBullet FireBullet;
  public UnityEvent OnFailedToShoot;

  private IntervalTimer FireTimer;

  void Start() {
    FireTimer = new IntervalTimer() {
      TimeClass = "PlayingTime"
    };

    FireTimer.Start();
  }

	void Update() {
    if (!StateManager.Playing)
      return;

    PlayerWeapon weapon = PlayerWeapon();

    FireTimer.Interval = weapon.CooldownInterval();

    if (WrappedInput.GetButton("Fire") && FireTimer.Elapsed()) {
      FireTimer.Reset();

      if (weapon.HasBullets()) {
        Fire(weapon);
      } else {
        OnFailedToShoot.Invoke();
      }
    }
	}

  void Fire(PlayerWeapon weapon) {
    weapon.ExpendBullet();

    FireBullet.Fire(
      prefab: weapon.BulletPrefab,
      getDirection: () => ScatterDirection.Scatter(
        ((IDirectionManager) AimDirection).Direction,
        amount: weapon.Scatter
      ),
      speed: weapon.BulletSpeed,
      damage: weapon.Damage,
      sound: weapon.ShootSound
    );
  }

  PlayerWeapon PlayerWeapon() 
    => gameObject.GetComponent<EquippedWeapon>().CurrentWeapon;
}
