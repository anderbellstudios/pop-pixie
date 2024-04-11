using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfindingGoal : MonoBehaviour {
  public int OpenDoorThreshold;
  public GameObject Door;
  public int Count = 0;
  public bool Destroys;

  void OnTriggerEnter2D(Collider2D other) {
    Count++;

    if (Door != null && Count == OpenDoorThreshold) {
      Destroy(Door);
      PathfindingGraph.Current.Recompute();
    }

    if (Destroys) {
      Destroy(other.gameObject);
    }
  }
}
