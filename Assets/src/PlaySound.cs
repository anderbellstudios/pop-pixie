using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {
  public bool PlayOnStart = false;
  public FMODUnity.StudioEventEmitter EventEmitter;

  void Start() {
    if (PlayOnStart) {
      Play();
    }
  }

  public void Play() {
    EventEmitter.Play();
  }
}
