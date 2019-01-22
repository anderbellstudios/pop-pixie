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

    float x_component = Input.GetAxis("Fire X");
    float y_component = Input.GetAxis("Fire Y");

    var direction = new Vector3(
      x_component, 
      y_component, 
      0
    ).normalized;

    var origin = gameObject.transform.position + direction;

    var bullet = Instantiate(
      prefab, 
      origin,
      transform.rotation
    );

    bullet.GetComponent<Rigidbody2D>().velocity = Speed * direction;
	}
}
