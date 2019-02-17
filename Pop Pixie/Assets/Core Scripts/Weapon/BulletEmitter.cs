using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {

  public MonoBehaviour DirectionManager;

  private DateTime LastShot;

	void Update () {
    if ( Input.GetButton("Fire1") && CanShoot() ) {
      Shoot();
    }
	}

  bool CanShoot () {
    var since = DateTime.Now.Subtract( LastShot ).TotalSeconds;
    if ( since <= CoolDownDuration() )
      return false;

    if ( !CurrentWeapon().HasBullets() )
      return false;

    return true;
  }
	
	void Shoot () {
    LastShot = DateTime.Now;

    CurrentWeapon().ExpendBullet();

    var dm = (WeaponDirectionManager) DirectionManager;
    var direction = dm.Direction;

    var origin = gameObject.transform.position + direction;

    var bullet = Instantiate(
      Prefab(), 
      origin,
      transform.rotation
    );

    bullet.GetComponent<Rigidbody2D>().velocity = Speed() * direction;
	}

  float Speed() {
    return CurrentWeapon().BulletSpeed;
  }

  float CoolDownDuration() {
    return 1.0f / CurrentWeapon().FireRate;
  }
  
  GameObject Prefab() {
    return CurrentWeapon().BulletPrefab;
  }

  Weapon CurrentWeapon() {
    return gameObject.GetComponent<EquippedWeapon>().CurrentWeapon;
  }
}
