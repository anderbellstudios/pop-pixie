using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingRingPull : MonoBehaviour {
  public PersistentId PersistentId;
  public SpawnFlyingRingPull SpawnFlyingRingPull;

  private string Id => PersistentId.Id;

  void Start() {
    if (ActivatedData.IsActivated(Id)) {
      Destroy(gameObject);
    }
  }

  void OnTriggerEnter2D(Collider2D col) {
    if (col.tag == "Player") {
      ActivatedData.RecordActivation(Id);
      SpawnFlyingRingPull.Instantiate();
      Destroy(gameObject);
    }
  }
}
