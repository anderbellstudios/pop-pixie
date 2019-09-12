using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : AFireable {

  public BulletEmitter BulletEmitter;

  public bool Firing;
  private IntervalTimer FireTimer;

  void Start() {
    FireTimer = new IntervalTimer() {
      Interval = CurrentWeapon().CooldownInterval()
    };
  }

  public override void BeginFiring() {
    Firing = true;
    FireTimer.Start();
  }

  public override void StopFiring() {
    Firing = false;
  }

  void Update() {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    if ( Firing )
      FireTimer.IfElapsed( () => ShootBullet() );
  }

  void ShootBullet() {
    BulletEmitter.Shoot( CurrentWeapon() );
  }

  Weapon CurrentWeapon() {
    return Weapon.Turret();
  }

}
