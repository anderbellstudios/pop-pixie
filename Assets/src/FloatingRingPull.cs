using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingRingPull : MonoBehaviour {

  public SpawnFlyingRingPull SpawnFlyingRingPull;

  void OnTriggerEnter2D(Collider2D col) {
    if (col.tag == "Player") {
      SpawnFlyingRingPull.Instantiate();
      Destroy(gameObject);
    }
  }

}
