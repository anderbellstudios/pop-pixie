using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHitPointEvents : MonoBehaviour {
  public HitPoints OverrideHitPoints;
  public Flash Flash;
  public DeathAnimation DeathAnimation;
  public Collider2D Collider;
  public HUDBar HealthBar;
  public bool HideHealthBarWhenFullOrEmpty;

  void Awake() {
    HitPoints hitPoints = OverrideHitPoints ?? GetComponent<HitPoints>();

    if (HealthBar != null) {
      hitPoints.OnUpdate.AddListener(hp => {
        float relativeHP = hp.Current / hp.Maximum;
        HealthBar.Progress = relativeHP;
        HealthBar.gameObject.SetActive(!HideHealthBarWhenFullOrEmpty || (relativeHP > 0 && relativeHP < 1));
      });
    }

    if (Flash != null) {
      hitPoints.OnDecrease.AddListener(hp => Flash.BeginFlashing());
    }

    hitPoints.OnBecomeZero.AddListener(hp => {
      DisableAIs();
      Collider.enabled = false;
      DeathAnimation.Play();
    });
  }

  void DisableAIs() {
    foreach (var ai in GetComponents<AEnemyAI>()) {
      if (ai.InControl)
        ai.RelinquishControl();
    }
  }
}
