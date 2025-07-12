using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollidesWithDamageable : MonoBehaviour {
  public BulletData BulletData;
  public Rigidbody2D Rigidbody;
  public Collider2D Collider;
  public GameObject Explosion;

  private bool Hit = false;
  private int ShouldDestroyInFrames = -1;

  void Start() {
    SetIgnoreCollisionWithSelf(true);
  }

  void OnCollisionEnter2D(Collision2D collision) {
    if (Hit)
      return;

    HitPoints hp = collision.gameObject.GetComponent<HitPoints>();

    if (hp != null) {
      bool isCounterAttack = hp.Damage(BulletData.Damage, true);

      if (isCounterAttack && BulletData.Originator) {
        Vector3 toOriginator = (
          BulletData.Originator.transform.position - transform.position
        ).normalized;
        Rigidbody.velocity = BulletData.CounterAttackSpeed * toOriginator;
        BulletData.Damage = BulletData.CounterAttackDamage;
        gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
        SetIgnoreCollisionWithSelf(false);
        return;
      }
    }

    Hit = true;

    Vector3 point = collision.GetContact(0).point;
    Instantiate(Explosion, point, Quaternion.identity);

    // Ensure bullet stays at contact point
    gameObject.transform.position = point;
    Rigidbody.simulated = false;
    Collider.enabled = false;

    /**
     * Give the trail renderer time to catch up, ensuring a continuous trail
     * that ends at the explosion.
     */
    ShouldDestroyInFrames = 2;
  }

  void Update() {
    if (ShouldDestroyInFrames == 0) {
      Destroy(gameObject);
    } else if (ShouldDestroyInFrames > 0) {
      ShouldDestroyInFrames--;
    }
  }

  private void SetIgnoreCollisionWithSelf(bool ignore) {
    if (BulletData.IgnoreCollider == null)
      return;

    Physics2D.IgnoreCollision(Collider, BulletData.IgnoreCollider, ignore);
  }
}
