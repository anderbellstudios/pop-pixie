using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour {
  public PlaySound PlaySound;
  public Collider2D IgnoreCollider;

  public void Fire(
    GameObject prefab,
    float speed,
    float damage,
    Func<Vector3> getDirection = null,
    Func<Vector3> getTarget = null,
    float? counterAttackDamage = null,
    Vector3? origin = null,
    string soundKey = ""
  ) {
#if UNITY_EDITOR
    if ((getDirection == null) == (getTarget == null)) {
      throw new System.Exception("FireBullet requires getDirection or getTarget, but not both");
    }
#endif

    Vector3 safeOrigin = origin ?? transform.position;

    if (getDirection == null) {
      getDirection = () => getTarget() - safeOrigin;
    }

    Vector3 direction = getDirection();

    if (direction.magnitude == 0)
      return;

    GameObject bullet = Instantiate(prefab, safeOrigin, Quaternion.identity);

    BulletData bulletData = bullet.GetComponent<BulletData>();
    bulletData.Damage = damage;
    bulletData.Originator = gameObject;
    bulletData.CounterAttackSpeed = speed;
    bulletData.CounterAttackDamage = counterAttackDamage ?? damage;
    bulletData.GetDirection = getDirection;
    bulletData.IgnoreCollider = IgnoreCollider;

    bullet.GetComponent<Rigidbody2D>().velocity = speed * direction.normalized;

    if (soundKey != "") {
      PlaySound.Play(soundKey);
    }
  }
}
