using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollidesWithEnemy : MonoBehaviour {

  void OnCollisionEnter2D (Collision2D col) {
    Destroy(gameObject);
  }

}
