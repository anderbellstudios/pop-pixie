using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHitPointEvents : MonoBehaviour, IHitPointEvents {

  public SoundController SoundPlayer;
  public float ChanceToPlaySound = 0f;
  public Flash Flash;
  public DeathAnimation DeathAnimation;
  public Collider2D Collider;
  public List<AudioClip> Sounds;

  public bool IsDead = false;

  public void Updated (HitPoints hp) {
  }

  public void Decreased (HitPoints hp) {
    if ( Flash != null ) Flash.BeginFlashing();

    if ( Sounds.Count > 0 && ShouldPlaySound() ) {
      // Play hurt sound
      int i = UnityEngine.Random.Range(0, Sounds.Count);
      var sound = Sounds[i];
      SoundPlayer.Play(sound);
    }
  }

  bool ShouldPlaySound() {
    return UnityEngine.Random.value < ChanceToPlaySound;
  }

  public void BecameZero (HitPoints hp) {
    IsDead = true;
    disableAIs();
    Collider.enabled = false;
    DeathAnimation.Play();
  }

  void disableAIs() {
    foreach ( var ai in GetComponents<AEnemyAI>() ) {
      if ( ai.InControl )
        ai.RelinquishControl();
    }
  }

}
