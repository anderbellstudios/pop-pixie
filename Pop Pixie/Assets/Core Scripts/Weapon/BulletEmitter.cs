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

    if (direction.magnitude == 0) {
      direction = new Vector3(0, 1, 0);
    }

    var bullet = Instantiate(
      Weapon.BulletPrefab,
      transform.position,
      transform.rotation
    );

    BulletData bulletData = bullet.GetComponent<BulletData>();
    bulletData.Damage = Weapon.Damage;
    bulletData.DirectionManager = dm;
    bullet.GetComponent<Rigidbody2D>().velocity = Speed() * direction.normalized;
  }

  void PlayPewSound() {
    SoundController.Play( Weapon.ShootSound );
  }

  float Speed() {
    return Weapon.BulletSpeed;
  }
  
}
