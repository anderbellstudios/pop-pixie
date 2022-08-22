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
  public bool HideHealthBarWhenFullOrEmpty;

  public void Updated (HitPoints hp) {
    if (HealthBar != null) {
      float relativeHP = hp.Current / hp.Maximum;
      HealthBar.Progress = relativeHP;
      HealthBar.gameObject.SetActive(!HideHealthBarWhenFullOrEmpty || (relativeHP > 0 && relativeHP < 1));
    }
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
