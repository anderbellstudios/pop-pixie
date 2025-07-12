using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackAI : ALegacyEnemyAI, IRequiresLineOfMovementAI {
  public BulletEmitter BulletEmitter;
  public Weapon Weapon;

  public ALegacyEnemyAI WhenAttackFinished;

  public override void ControlGained() {
    PerformAttack();
    RelinquishControlTo(WhenAttackFinished);
  }

  void PerformAttack() {
    BulletEmitter.Shoot(Weapon);
  }
}
