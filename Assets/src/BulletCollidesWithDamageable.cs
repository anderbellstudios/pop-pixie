using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollidesWithDamageable : MonoBehaviour {
  public BulletData BulletData;
  public Collider2D Collider;
  public GameObject Explosion;

  void Start() {
    SetIgnoreCollisionWithSelf(true);
  }

  void OnCollisionEnter2D(Collision2D col) {
    HitPoints hp = col.gameObject.GetComponent<HitPoints>();

    if (hp != null) {
      bool isCounterAttack = hp.Damage(BulletData.Damage, true);

      if (isCounterAttack && BulletData.Originator) {
        Vector3 toOriginator = (BulletData.Originator.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = BulletData.CounterAttackSpeed * toOriginator;
        BulletData.Damage = BulletData.CounterAttackDamage;
        gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
        SetIgnoreCollisionWithSelf(false);
        return;
      }
    }

    Instantiate(Explosion, transform.position, Quaternion.identity);
    Destroy(gameObject);
  }

  private void SetIgnoreCollisionWithSelf(bool ignore) {
    if (BulletData.IgnoreCollider == null)
      return;

    Physics2D.IgnoreCollision(Collider, BulletData.IgnoreCollider, ignore);
  }
}
