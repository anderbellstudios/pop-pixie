using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {

  public GameObject prefab;
  public MonoBehaviour DirectionManager;
  public float CoolDownDuration;
  public float Speed;
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
    return since > CoolDownDuration;
  }
	
	void Shoot () {
    LastShot = DateTime.Now;

    var dm = (WeaponDirectionManager) DirectionManager;
    var direction = dm.Direction;

    var origin = gameObject.transform.position + direction;

    var bullet = Instantiate(
      prefab, 
      origin,
      transform.rotation
    );

    bullet.GetComponent<Rigidbody2D>().velocity = Speed * direction;
	}
}
