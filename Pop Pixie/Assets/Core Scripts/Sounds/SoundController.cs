using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PauseBehaviourEnum { Ignore, Pause, Interrupt };

public class SoundController : MonoBehaviour {
  public AudioSource Player;
  public float BaseVolume = 1f;
  public bool OneShot = false;
  public PauseBehaviourEnum PauseBehaviour = PauseBehaviourEnum.Pause;
  public bool UseVoiceVolume = false;

  private float SoundVolume = 1f;

  public void Play (AudioClip sound, float volume = 1f) {
    SoundVolume = volume;

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
    Player.volume = BaseVolume
      * SoundVolume
      * AudioFadeOut.Current.FadeLevel()
      * (float)(UseVoiceVolume ? OptionsData.VoiceVolume : OptionsData.SoundsVolume);

    if (PauseBehaviour != PauseBehaviourEnum.Ignore && StateManager.Enabled(StateFeatures.InterruptSounds) && Player.isPlaying) {
      if (PauseBehaviour == PauseBehaviourEnum.Pause) {
        Player.Pause();
        Paused = true;
      } else {
        Stop();
      }
    }

    if (PauseBehaviour == PauseBehaviourEnum.Pause && !StateManager.Enabled(StateFeatures.InterruptSounds) && Paused) {
      Player.Play();
      Paused = false;
    }
  }
}
