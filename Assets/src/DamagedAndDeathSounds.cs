using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedAndDeathSounds : MonoBehaviour, IHitPointEvents {
  public SoundController SoundController;
  public List<AudioClip> DamagedSounds, DeathSounds;

  public void Updated (HitPoints hp) {}

  public void Decreased (HitPoints hp) {
    PlayRandomSound(DamagedSounds);
  }

  public void BecameZero (HitPoints hp) {
    PlayRandomSound(DeathSounds);
  }

  private void PlayRandomSound(List<AudioClip> sounds) {
    if (sounds.Count > 0) {
      int i = UnityEngine.Random.Range(0, sounds.Count);
      AudioClip sound = sounds[i];
      SoundController.Play(sound);
    }
  }
}
