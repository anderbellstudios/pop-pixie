using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMusicController : MonoBehaviour {
  public GameObject Prefab;

  void Awake() {
    if (MusicController.Current == null) {
      Instantiate(Prefab);
    }
  }
}
