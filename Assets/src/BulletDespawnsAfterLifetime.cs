using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDespawnsAfterLifetime : MonoBehaviour {
  public BulletData BulletData;

  void Start() {
    AsyncTimer.PlayingTime.SetTimeout(
      () => {
        Destroy(gameObject);
      },
      BulletData.Lifetime,
      gameObject
    );
  }
}
