using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour {
  public PlaySound PlaySound;

  public void Fire(GameObject prefab, Func<Vector3> getDirection, float speed, float damage, string soundKey = "") {
    Vector3 direction = getDirection();

    if (direction.magnitude == 0)
      return;

    GameObject bullet = Instantiate(prefab, transform.position, transform.rotation);

    BulletData bulletData = bullet.GetComponent<BulletData>();
    bulletData.Damage = damage;
    bulletData.Originator = gameObject;
    bulletData.CounterAttackSpeed = speed;
    bulletData.GetDirection = getDirection;

    bullet.GetComponent<Rigidbody2D>().velocity = speed * direction.normalized;

    if (soundKey != "") {
      PlaySound.Play(soundKey);
    }
  }
}
