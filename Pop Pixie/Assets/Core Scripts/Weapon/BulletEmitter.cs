using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {

  public MonoBehaviour DirectionManager;
  public bool ShouldPlayPewSound = false;
  public SoundController SoundController;

  private Weapon Weapon;

	public void Shoot( Weapon weapon ) {
    Weapon = weapon;

    InstantiateBullet();
    if ( ShouldPlayPewSound ) PlayPewSound();
  }

  void InstantiateBullet() {
    var dm = (IDirectionManager) DirectionManager;
    var direction = dm.Direction;

    var bullet = Instantiate(
      Weapon.BulletPrefab,
      transform.position,
      transform.rotation
    );

    bullet.GetComponent<Rigidbody2D>().velocity = Speed() * direction;
	}

  void PlayPewSound() {
    SoundController.Play( Weapon.ShootSound );
  }

  float Speed() {
    return Weapon.BulletSpeed;
  }
  
}
