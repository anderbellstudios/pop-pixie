using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
  public AudioSource Player;
  public float BaseVolume = 1f;
  public bool OneShot = false;

  public void Play (AudioClip sound, float volume = 1f) {
    Player.volume = BaseVolume * ((float) OptionsData.SoundsVolume) * volume;

    if ( OneShot ) {
      Player.PlayOneShot(sound);
    } else {
      Player.clip = sound;
      Player.Play();
    }
  }

  public void Stop() {
    Player.Stop();
  }

  private bool Paused = false;

  void Update() {
    if (StateManager.Is(State.Paused) && Player.isPlaying) {
      Player.Pause();
      Paused = true;
    }

    if (StateManager.Isnt(State.Paused) && Paused) {
      Player.Play();
      Paused = false;
    }
  }
}
