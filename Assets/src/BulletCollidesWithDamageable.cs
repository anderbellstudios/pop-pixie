using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollidesWithDamageable : MonoBehaviour {

  public BulletData BulletData;
  public GameObject Explosion;

  void OnCollisionEnter2D(Collision2D col) {
    HitPoints hp = col.gameObject.GetComponent<HitPoints>();

    if (hp != null) {
      bool isCounterAttack = hp.Damage(BulletData.Damage, true);

      if (isCounterAttack && BulletData.Originator) {
        Vector3 toOriginator = (BulletData.Originator.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = BulletData.CounterAttackSpeed * toOriginator;
        gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
        return;
      }
    }

    ImmoboliseBullet();
    SpawnExplosion();
    Invoke("DestroyBullet", 0.5f);
  }

  void ImmoboliseBullet() {
    GetComponent<SpriteRenderer>().enabled = false;
    GetComponent<Collider2D>().enabled = false;
    GetComponent<Rigidbody2D>().simulated = false;
    GetComponent<TrailRenderer>().emitting = false;
  }

  void SpawnExplosion() {
    Instantiate(Explosion, transform);
  }

  void DestroyBullet() {
    Destroy(gameObject);
  }

}
