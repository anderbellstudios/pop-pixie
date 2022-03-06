using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHitPointEvents : MonoBehaviour, IHitPointEvents {

  public Flash Flash;
  public DeathAnimation DeathAnimation;
  public Collider2D Collider;
  public HUDBar HealthBar;

  public void Updated (HitPoints hp) {
    if (HealthBar != null)
      HealthBar.Progress = hp.Current / hp.Maximum;
  }

  public void Decreased (HitPoints hp) {
    if ( Flash != null ) Flash.BeginFlashing();
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
