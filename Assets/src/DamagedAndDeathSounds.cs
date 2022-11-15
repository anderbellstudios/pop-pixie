using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedAndDeathSounds : MonoBehaviour {
  public HitPoints OverrideHitPoints;
  public SoundController SoundController;
  public List<AudioClip> DamagedSounds, DeathSounds;

  void Awake() {
    HitPoints hitPoints = OverrideHitPoints ?? GetComponent<HitPoints>();

    hitPoints.OnDecrease.AddListener(hp => {
      PlayRandomSound(DamagedSounds);
    });

    hitPoints.OnBecomeZero.AddListener(hp => {
      PlayRandomSound(DeathSounds);
    });
  }

  private void PlayRandomSound(List<AudioClip> sounds) {
    if (sounds.Count > 0) {
      int i = UnityEngine.Random.Range(0, sounds.Count);
      AudioClip sound = sounds[i];
      SoundController.Play(sound);
    }
  }
}
