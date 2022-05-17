using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologramBombAttackAI : AEnemyAI {
  public GameObject BombPrefab;

  public AEnemyAI WhenFinished;

  public override void ControlGained() {
    InvokeRepeating("Fire", 1f, 1f);
    // RelinquishControlTo(WhenFinished);
  }

  void Fire() {
    if (!StateManager.Playing)
      return;

    Instantiate(
      BombPrefab,
      transform.position,
      transform.rotation
    ).GetComponent<Rigidbody2D>().velocity = TargetHeading();
  }
}
