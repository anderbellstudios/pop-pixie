using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDespawnsAfterLifetime : MonoBehaviour {

  public BulletData BulletData;

  void Start() {
    Invoke("DestroyBullet", BulletData.Lifetime);
  }

  void DestroyBullet() {
    Destroy(gameObject);
  }

}
