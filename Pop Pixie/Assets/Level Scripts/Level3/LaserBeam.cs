using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {

  public float Damage;
  private float Angle; 
  private RaycastHit2D CachedHitData;

  public void Fire( float angle ) {
    Angle = angle;
    CachedHitData = HitData();
    DrawDebugRay();

    if ( HitGameObjectIsPlayer() ) {
      HitGameObject().GetComponent<HitPoints>().Damage(Damage);
    }
  }

  public RaycastHit2D HitData() {
    return Physics2D.Raycast(
      transform.position,
      Direction()
    );
  }

  GameObject HitGameObject() {
    return CachedHitData.collider.gameObject;
  }

  bool HitGameObjectIsPlayer() {
    return HitGameObject().tag == "Player";
  }

  void DrawDebugRay() {
    Debug.DrawRay(
      transform.position,
      Heading(),
      Color.red
    );
  }

  Vector3 Heading() {
    return HitDistance() * Direction();
  }

  float HitDistance() {
    return CachedHitData.distance;
  }

  Vector3 Direction() {
    return Rotation() * Vector3.right;
  }

  Quaternion Rotation() {
    return Quaternion.AngleAxis( Angle, Vector3.forward );
  }

}
