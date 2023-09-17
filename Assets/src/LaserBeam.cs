using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {

  public float Damage;
  private float Angle;
  private RaycastHit2D CachedHitData;

  public Vector3 Fire(float angle) {
    Angle = angle;
    CachedHitData = HitData();

    if (HitGameObjectIsPlayer()) {
      HitGameObject().GetComponent<HitPoints>().Damage(Damage);
    }

    return Heading();
  }

  public RaycastHit2D HitData() => Physics2D.Raycast(
    transform.position,
    Direction(),
    Mathf.Infinity,
    IgnoreEnemyLayerMask.Mask
  );

  GameObject HitGameObject()
    => CachedHitData.collider?.gameObject;

  bool HitGameObjectIsPlayer()
    => HitGameObject()?.tag == "Player";

  Vector3 Heading()
    => HitDistance() * Direction();

  float HitDistance()
    => CachedHitData.distance;

  Vector3 Direction()
    => Rotation() * Vector3.right;

  Quaternion Rotation()
    => Quaternion.AngleAxis(Angle, Vector3.forward);

}
