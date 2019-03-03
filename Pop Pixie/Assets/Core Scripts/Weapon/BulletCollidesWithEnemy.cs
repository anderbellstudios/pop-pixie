using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollidesWithEnemy : MonoBehaviour {

  public float Damage;

  void OnCollisionEnter2D (Collision2D col) {
    Destroy(gameObject);

    var obj = col.gameObject;
    if ( obj.tag == "Enemy" ) {
      obj.GetComponent<HitPoints>().Damage( Damage );
    }
  }

}
