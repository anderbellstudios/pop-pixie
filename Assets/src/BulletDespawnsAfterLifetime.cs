using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDespawnsAfterLifetime : MonoBehaviour {

  public BulletData BulletData;

  private IntervalTimer DespawnTimer;

  void Start() {
    DespawnTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = BulletData.Lifetime
    };

    DespawnTimer.Reset();
  }

  void Update() {
    DespawnTimer.IfElapsed(DestroyBullet);
  }

  void DestroyBullet() {
    Destroy(gameObject);
  }

}
