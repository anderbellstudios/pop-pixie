using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PauseBehaviourEnum { Ignore, Pause, Interrupt };

public class SoundController : MonoBehaviour {
  public AudioSource Player;
  public float BaseVolume = 1f;
  public bool OneShot = false;
  public PauseBehaviourEnum PauseBehaviour = PauseBehaviourEnum.Pause;

  private LowPriorityBehaviour LowPriorityBehaviour;
  private float SoundVolume;

  void Awake() {
    LowPriorityBehaviour = new LowPriorityBehaviour();
  }

  public void Play (AudioClip sound, float volume = 1f) {
    SoundVolume = volume;
    UpdateVolume();

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
    LowPriorityBehaviour.EveryNFrames(10, UpdateVolume);

    if (PauseBehaviour != PauseBehaviourEnum.Ignore && StateManager.Is(State.Paused) && Player.isPlaying) {
      if (PauseBehaviour == PauseBehaviourEnum.Pause) {
        Player.Pause();
        Paused = true;
      } else {
        Stop();
      }
    }

    if (PauseBehaviour == PauseBehaviourEnum.Pause && StateManager.Isnt(State.Paused) && Paused) {
      Player.Play();
      Paused = false;
    }
  }

  void UpdateVolume() {
    Player.volume = BaseVolume * ((float) OptionsData.SoundsVolume) * SoundVolume;
  }
}
