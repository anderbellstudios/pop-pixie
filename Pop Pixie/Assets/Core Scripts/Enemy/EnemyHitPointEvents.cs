﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHitPointEvents : MonoBehaviour, IHitPointEvents {

  public SoundController SoundPlayer;
  public Flash Flash;
  public DeathAnimation DeathAnimation;
  public Collider2D Collider;
  public List<AudioClip> Sounds;

  public void Updated (HitPoints hp) {
  }

  public void Decreased (HitPoints hp) {
    if ( Flash != null ) Flash.BeginFlashing();

    if ( Sounds.Count > 0 ) {
      // Play hurt sound
      int i = UnityEngine.Random.Range(0, Sounds.Count);
      var sound = Sounds[i];
      SoundPlayer.Play(sound);
    }
  }

  public void BecameZero (HitPoints hp) {
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
