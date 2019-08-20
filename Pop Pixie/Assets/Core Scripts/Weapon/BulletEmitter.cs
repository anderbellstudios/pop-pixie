using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {

  public MonoBehaviour DirectionManager;

  private Weapon Weapon;

	public void Shoot( Weapon weapon ) {
    Weapon = weapon;

    Weapon.ExpendBullet();

    var dm = (IDirectionManager) DirectionManager;
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
    return Weapon.BulletSpeed;
  }
  
  GameObject Prefab() {
    return Weapon.BulletPrefab;
  }
}
