using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

  public BulletEmitter BulletEmitter;
  public DirectionScatterer DirectionScatterer;
  public SoundController SoundController;
  public AudioClip FailToShootSound;

  private IntervalTimer FireTimer;

  void Start () {
    FireTimer = new IntervalTimer();
    FireTimer.Start();
  }

	void Update () {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    Weapon weapon = CurrentWeapon().Weapon;

    FireTimer.Interval = weapon.CooldownInterval();

    if ( WrappedInput.GetButton("Fire") && CanShoot() ) {
      FireTimer.Reset();
      CurrentWeapon().ExpendBullet();
      DirectionScatterer.Angle = weapon.Scatter;
      BulletEmitter.Shoot( weapon );
    } else if ( WrappedInput.GetButtonDown("Fire") && !CurrentWeapon().HasBullets() ) {
      SoundController.Play( FailToShootSound, 0.1f );
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
