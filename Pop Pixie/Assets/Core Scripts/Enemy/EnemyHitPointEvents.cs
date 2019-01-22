using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitPointEvents : MonoBehaviour, IHitPointEvents {
  public void Decreased (HitPoints hp) {
  }

  public void BecameZero (HitPoints hp) {
    Destroy(gameObject);
  }
}
