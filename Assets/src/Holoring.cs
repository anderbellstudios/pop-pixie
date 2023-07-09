using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holoring : MonoBehaviour {
  public float ExpandSpeed;
  public float Lifetime;
  public Action<float> DamageBoss;

  private bool IsCounterAttacked = false;

  void Update() {
    if (!StateManager.Playing)
      return;

    float delta = Time.deltaTime * ExpandSpeed * (IsCounterAttacked ? -1 : 1);
    transform.localScale += new Vector3(delta, delta, 0);

    if (transform.localScale.x < 0) {
      DamageBoss(50);
      Destroy(gameObject);
      return;
    }

    Lifetime -= Time.deltaTime;

    if (Lifetime <= 0)
      Destroy(gameObject);
  }

  public void HandleCollidedWithPlayer() {
    if (IsCounterAttacked) return;
    IsCounterAttacked = PlayerGameObject.Current.GetComponent<HitPoints>().Damage(1, true);
  }
}
