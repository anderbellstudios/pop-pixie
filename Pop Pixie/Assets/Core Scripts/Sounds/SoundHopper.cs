using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHopper : MonoBehaviour {
  public SoundController SoundController;
  public AudioClip AudioClip;

  public void Hop() {
    SoundController.Play(AudioClip);
  }
}
