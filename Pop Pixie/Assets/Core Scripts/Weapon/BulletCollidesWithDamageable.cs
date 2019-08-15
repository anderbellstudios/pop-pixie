using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollidesWithDamageable : MonoBehaviour {

  public float Damage;

  void OnCollisionEnter2D (Collision2D col) {
    Destroy(gameObject);

    var hp = col.gameObject.GetComponent<HitPoints>();

    if ( hp != null ) {
      hp.Damage( Damage );
    }
  }

}
