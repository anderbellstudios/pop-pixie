using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologramBombAttackAI : ARepeatedAttackAI {
  public GameObject BombPrefab;
  public float BombVelocityCoefficient;

  public override void PerformAttack() {
    Instantiate(
      BombPrefab,
      transform.position,
      transform.rotation
    ).GetComponent<Rigidbody2D>().velocity = BombVelocityCoefficient * TargetHeading();
  }
}
