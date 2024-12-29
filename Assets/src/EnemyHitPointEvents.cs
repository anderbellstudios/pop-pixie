using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitPointEvents : MonoBehaviour {
  [UnityEngine.Serialization.FormerlySerializedAs("OverrideHitPoints")]
  public HitPoints HitPoints;

  public Flash Flash;
  public AEnemyAI RootAI;
  public DeathAnimation DeathAnimation;
  public Collider2D Collider;

  public HUDBar HealthBar;
  public bool HideHealthBarWhenFullOrEmpty;

  public bool SlowOnDamage = true;
  public float SlowOnDamageDuration;
  public MovementManager MovementManager;

  private Stopwatch SlowStopwatch = null;

  void Awake() {
    if (HitPoints == null) {
      Debug.LogWarning("Automatically attaching HitPoints component is deprecated");
      HitPoints = GetComponent<HitPoints>();
    }

    HitPoints.OnUpdate.AddListener(hp => {
      if (HealthBar != null) {
        float relativeHP = hp.Current / hp.Maximum;
        HealthBar.SetProgress(relativeHP);
        HealthBar.SetVisible(!HideHealthBarWhenFullOrEmpty || (relativeHP > 0 && relativeHP < 1));
      }
    });

    HitPoints.OnDecrease.AddListener(hp => {
      Flash?.BeginFlashing();

      if (SlowOnDamage) {
        SlowStopwatch = new Stopwatch.PlayingTime();
      }
    });

    HitPoints.OnBecomeZero.AddListener(hp => {
      DisableAIs();
      Collider.enabled = false;
      DeathAnimation?.Play();
    });

    MovementManager?.SpeedModifiers.Add((speed) => {
      if (!SlowOnDamage || SlowStopwatch == null)
        return speed;

      float progress = SlowStopwatch.Progress(SlowOnDamageDuration);
      float factor = (1f - Mathf.Cos(progress * Mathf.PI)) / 2f;

      if (progress >= 1f) {
        SlowStopwatch = null;
      }

      return speed * factor;
    });
  }

  void DisableAIs() {
    RootAI?.Deactivate();

    foreach (var ai in GetComponents<ALegacyEnemyAI>()) {
      if (ai.InControl)
        ai.RelinquishControl();
    }
  }
}
