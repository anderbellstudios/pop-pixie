using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooksAtPlayer : MonoBehaviour {

  public GameObject player;
  public float k, w_squared;
  public float Velocity;

	void Update () {
    Accelerate( w_squared * RelativeAngle() );
    Accelerate( -k * Velocity );

    VelocityTick();
	}

  void Accelerate( float amount ) {
    Velocity += amount * Time.deltaTime;
  }

  void VelocityTick() {
    transform.Rotate( 0, 0, Velocity * Time.deltaTime );
  }

  float RelativeAngle() {
    return Mathf.Deg2Rad * Vector3.SignedAngle(
      transform.right, 
      TargetDirection(), 
      Vector3.forward
    );
  }

  Vector3 TargetDirection() {
    return player.transform.position - transform.position;
  }
}
