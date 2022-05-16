using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHopper : MonoBehaviour {
  public SoundController SoundController;
  public AudioClip AudioClip;
  public bool PlayOnAwake = false;

  void Awake() {
    if (PlayOnAwake)
      Hop();
  }

  public void Hop() {
    SoundController.Play(AudioClip);
  }
}
