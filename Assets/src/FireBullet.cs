using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour {
  public SoundController SoundController;

  public void Fire(GameObject prefab, Func<Vector3> getDirection, float speed, float damage, AudioClip sound = null) {
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

    if (sound != null)
      SoundController.Play(sound);
  }
}
