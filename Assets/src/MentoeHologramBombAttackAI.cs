using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologramBombAttackAI : ARepeatedAttackAI {
  public GameObject BombPrefab;
  public float PredictiveAimLeadTime;
  public float BombVelocityCoefficient;

  void Start() {
    HitPoints.PlayerHitPoints.OnDecrease.AddListener(hitPoints => {
      if (InControl) {
        EndAttack();
      }
    });
  }

  public override void PerformAttack() {
    Vector3 predictedPlayerPosition = ((Vector3)TargetHeading()) + (PlayerGameObject.EstimatedVelocity * PredictiveAimLeadTime);

    GameObject bombGameObject = Instantiate(
      BombPrefab,
      transform.position,
      transform.rotation
    );

    bombGameObject.GetComponent<Rigidbody2D>().velocity = BombVelocityCoefficient * predictedPlayerPosition;
    BulletData bulletData = bombGameObject.GetComponent<BulletData>();
    bulletData.Originator = gameObject;
    bulletData.Damage = 100;
  }
}
