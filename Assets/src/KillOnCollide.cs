using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCollide : MonoBehaviour {
  public bool PlayerOnly;

  void OnTriggerEnter2D(Collider2D collider) {
    OnCollideWithGameObject(collider.gameObject);
  }

  void OnCollisionEnter2D(Collision2D collision) {
    OnCollideWithGameObject(collision.gameObject);
  }

  private void OnCollideWithGameObject(GameObject gameObject) {
    if (PlayerOnly && gameObject.tag != "Player")
      return;

    HitPoints hitPoints = gameObject.GetComponent<HitPoints>();

    hitPoints?.Damage(
      Mathf.Infinity,
      ignoreCanBeDamaged: true,
      ignoreDamageReduction: true
    );
  }
}
