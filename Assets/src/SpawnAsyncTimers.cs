using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsyncTimers : MonoBehaviour {
  public GameObject Prefab;

  void Awake() {
    if (AsyncTimer.BaseTime == null) {
      Instantiate(Prefab);
    }
  }
}
