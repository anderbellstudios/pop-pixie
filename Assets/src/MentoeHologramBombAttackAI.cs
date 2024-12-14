using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologramBombAttackAI : ARepeatedAttackAI {
  public GameObject BombPrefab;
  public float PredictiveAimLeadTime;

  void Start() {
    HitPoints.PlayerHitPoints.OnDecrease.AddListener(hitPoints => {
      if (IsActive) {
        OnFinish();
      }
    });
  }

  protected override void PerformAttack() {
    Vector3 toPlayer = Helper.VectorToPlayer;

    bool predictPosition = Random.value < 0.75f;

    if (predictPosition) {
      toPlayer += PlayerGameObject.EstimatedVelocity * PredictiveAimLeadTime;
    }

    GameObject bombGameObject = Instantiate(
      BombPrefab,
      transform.position,
      transform.rotation
    );

    Rigidbody2D rb = bombGameObject.GetComponent<Rigidbody2D>();

    rb.velocity = DragUtils.VelocityForDisplacement(
      displacement: toPlayer,
      drag: rb.drag
    );

    BulletData bulletData = bombGameObject.GetComponent<BulletData>();
    bulletData.Originator = gameObject;
    bulletData.Damage = 100;
  }
}
