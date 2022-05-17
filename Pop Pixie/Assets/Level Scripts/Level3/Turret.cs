using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : AFireable {

  public Weapon Weapon;
  public BulletEmitter BulletEmitter;

  public bool Firing;
  private IntervalTimer FireTimer;

  void Start() {
    FireTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = Weapon.CooldownInterval()
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
    if (!StateManager.Playing)
      return;

    if ( Firing )
      FireTimer.IfElapsed( () => ShootBullet() );
  }

  void ShootBullet() {
    BulletEmitter.Shoot( Weapon );
  }

}
