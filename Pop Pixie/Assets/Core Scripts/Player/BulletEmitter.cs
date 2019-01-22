using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {

  public GameObject prefab;
  public float Speed;

	// Use this for initialization
	void Start () {
    InvokeRepeating("Shoot", 0.0f, 0.5f);
	}
	
	void Shoot () {
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
