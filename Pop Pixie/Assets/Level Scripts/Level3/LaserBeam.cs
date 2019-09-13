using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {

  public float Damage;
  private float Angle; 
  private RaycastHit2D CachedHitData;

  public Vector3 Fire( float angle ) {
    Angle = angle;
    CachedHitData = HitData();

    if ( HitGameObjectIsPlayer() ) {
      HitGameObject().GetComponent<HitPoints>().Damage(Damage);
    }

    return Heading();
  }

  public RaycastHit2D HitData() {
    return Physics2D.Raycast(
      transform.position,
      Direction(),
      Mathf.Infinity,
      Physics2D.DefaultRaycastLayers & ~( 1 << 8 ) // exclude the enemy layer
    );
  }

  GameObject HitGameObject() {
    return CachedHitData.collider.gameObject;
  }

  bool HitGameObjectIsPlayer() {
    return HitGameObject().tag == "Player";
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
