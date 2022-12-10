using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStateManager : MonoBehaviour {
  public GameObject Prefab;

  void Awake() {
    if (StateManager.Current == null) {
      Instantiate(Prefab);
    }
  }
}
