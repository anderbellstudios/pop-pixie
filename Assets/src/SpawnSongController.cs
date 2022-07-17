using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSongController : MonoBehaviour {
  public GameObject Prefab;

  void Awake() {
    if (SongController.Current == null) {
      Instantiate(Prefab);
    }
  }
}
