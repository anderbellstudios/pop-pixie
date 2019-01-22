using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {

  public GameObject prefab;
  public float CoolDownDuration;
  public float Speed;

  private DateTime LastShot;

	void Update () {
    if ( Input.GetButton("Fire1") && CanShoot() ) {
      Shoot();
    }
	}

  bool CanShoot () {
    var since = DateTime.Now.Subtract( LastShot ).TotalSeconds;
    return since > CoolDownDuration;
  }
	
	void Shoot () {
    LastShot = DateTime.Now;

    var direction = new Vector3( 0, 1, 0 ).normalized;
    var origin = gameObject.transform.position + ( 1 * direction );

    var bullet = Instantiate(
      prefab, 
      origin,
      Quaternion.identity
    );

    bullet.GetComponent<Rigidbody2D>().velocity = Speed * direction;
	}
}
