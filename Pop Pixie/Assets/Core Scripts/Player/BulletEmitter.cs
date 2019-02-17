using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {

  public MonoBehaviour DirectionManager;
  public Weapon EquippedWeapon;

  private DateTime LastShot;

  void Start () {
    EquippedWeapon = Weapon.PopPistol();
  }

	void Update () {
    if ( Input.GetButton("Fire1") && CanShoot() ) {
      Shoot();
    }
	}

  bool CanShoot () {
    var since = DateTime.Now.Subtract( LastShot ).TotalSeconds;
    return since > CoolDownDuration();
  }
	
	void Shoot () {
    LastShot = DateTime.Now;

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
    return EquippedWeapon.BulletSpeed;
  }

  float CoolDownDuration() {
    return 1.0f / EquippedWeapon.FireRate;
  }
  
  GameObject Prefab() {
    return EquippedWeapon.BulletPrefab;
  }
}
