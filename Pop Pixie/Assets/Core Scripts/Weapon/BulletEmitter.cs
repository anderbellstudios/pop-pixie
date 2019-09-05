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

    var bullet = Instantiate(
      Prefab(), 
      transform.position,
      transform.rotation
    );

    bullet.GetComponent<Rigidbody2D>().velocity = Speed() * direction;
	}

  float Speed() {
    return Weapon.BulletSpeed;
  }
  
  GameObject _Prefab = null;

  GameObject Prefab() {
    if ( _Prefab == null ) {
      _Prefab = (GameObject) Resources.Load(
        Weapon.BulletPrefab, 
        typeof(GameObject)
      );
    }

    return _Prefab;
  }
}
