using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfindingGoal : MonoBehaviour {
  public int OpenDoorThreshold;
  public GameObject Door;
  public int Count = 0;

  void OnTriggerEnter2D(Collider2D other) {
    if (++Count == OpenDoorThreshold) {
      Destroy(Door);
      PathfindingGraph.Current.Recompute();
    }

    Destroy(other.gameObject);
  }
}
