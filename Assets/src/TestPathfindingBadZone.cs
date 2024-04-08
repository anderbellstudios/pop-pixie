using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfindingBadZone : MonoBehaviour {
  public bool Failed = false;

  void OnTriggerEnter2D(Collider2D other) {
    Failed = true;
  }
}
