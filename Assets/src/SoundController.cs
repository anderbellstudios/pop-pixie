using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
  public AudioSource Player;
  public float BaseVolume = 1f;
  public bool OneShot = false;
  public bool UseVoiceVolume = false;
  public bool PlayWhilePaused = false;

  private float SoundVolume = 1f;

  void Awake() {
    Player.ignoreListenerPause = PlayWhilePaused;
  }

  public void Play(AudioClip sound, float volume = 1f) {
    SoundVolume = volume;

    if (OneShot) {
      Player.PlayOneShot(sound);
    } else {
      Player.clip = sound;
      Player.Play();
    }
  }

  public void Stop() {
    Player.Stop();
  }

  void Update() {
    Player.volume = BaseVolume
      * SoundVolume
      * AudioFadeOut.Current.FadeLevel()
      * (float)(UseVoiceVolume ? OptionsData.VoiceVolume : OptionsData.SoundsVolume);
  }
}
