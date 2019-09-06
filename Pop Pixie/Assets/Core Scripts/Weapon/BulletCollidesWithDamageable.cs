using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollidesWithDamageable : MonoBehaviour {

  public float Damage;

  void OnCollisionEnter2D (Collision2D col) {
    ImmoboliseBullet();
    SpawnExplosion();
    Invoke( "DestroyBullet", 0.5f );

    var hp = col.gameObject.GetComponent<HitPoints>();

    if ( hp != null ) {
      hp.Damage( Damage );
    }
  }

  void ImmoboliseBullet() {
    GetComponent<SpriteRenderer>().enabled = false;
    GetComponent<Collider2D>().enabled = false;
    GetComponent<Rigidbody2D>().simulated = false;
    GetComponent<TrailRenderer>().emitting = false;
  }

  void SpawnExplosion() {
    var explosion = (GameObject) Resources.Load(
      "Bullets/Explosion", 
      typeof(GameObject)
    );

    Instantiate(explosion, transform);
  }

  void DestroyBullet() {
    Destroy(gameObject);
  }

}
