using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour {

  public BulletEmitter BulletEmitter;
  public DirectionScatterer DirectionScatterer;
  public UnityEvent OnFailedToShoot;

  private IntervalTimer FireTimer;

  void Start () {
    FireTimer = new IntervalTimer() {
      TimeClass = "PlayingTime"
    };

    FireTimer.Start();
  }

	void Update () {
    if (!StateManager.Playing)
      return;

    Weapon weapon = CurrentWeapon().Weapon;

    FireTimer.Interval = weapon.CooldownInterval();

    if ( WrappedInput.GetButton("Fire") && CanShoot() ) {
      FireTimer.Reset();
      CurrentWeapon().ExpendBullet();
      DirectionScatterer.Angle = weapon.Scatter;
      BulletEmitter.Shoot( weapon );
    } else if ( WrappedInput.GetButtonDown("Fire") && !CurrentWeapon().HasBullets() ) {
      OnFailedToShoot.Invoke();
    }
	}

  bool CanShoot () {
    if ( !FireTimer.Elapsed() )
      return false;

    if ( !CurrentWeapon().HasBullets() )
      return false;

    return true;
  }

  PlayerWeapon CurrentWeapon() 
    => gameObject.GetComponent<EquippedWeapon>().CurrentWeapon;
}
