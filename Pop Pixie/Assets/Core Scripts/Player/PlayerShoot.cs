using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

  public BulletEmitter BulletEmitter;

  private IntervalTimer FireTimer;

  void Start () {
    FireTimer = new IntervalTimer() {
      Interval = 1.0f / CurrentWeapon().FireRate
    };

    FireTimer.Start();
  }

	void Update () {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    if ( Input.GetButton("Fire1") && CanShoot() ) {
      FireTimer.Reset();
      BulletEmitter.Shoot( CurrentWeapon() );
    }
	}

  bool CanShoot () {
    if ( !FireTimer.Elapsed() )
      return false;

    if ( !CurrentWeapon().HasBullets() )
      return false;

    return true;
  }

  Weapon CurrentWeapon() {
    return gameObject.GetComponent<EquippedWeapon>().CurrentWeapon;
  }
}
