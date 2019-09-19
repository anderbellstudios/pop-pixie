using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
  public AudioSource Player;

  public void Play (AudioClip sound) {
    Player.clip = sound;
    Player.Play();
  }

  public void Stop() {
    Player.Stop();
  }

  void Update() {
    Player.volume = (float) OptionsData.SoundsVolume;
  }
}
