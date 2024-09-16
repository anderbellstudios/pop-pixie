using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHitPointEvents : MonoBehaviour {
  public HitPoints OverrideHitPoints;
  public HUDBar HealthBar;
  public string GameOverScene = "Game Over";

  void Awake() {
    HitPoints hitPoints = OverrideHitPoints ?? GetComponent<HitPoints>();

    hitPoints.OnUpdate.AddListener(hp => {
      HealthBar.Progress = hp.Current / hp.Maximum;
    });

    hitPoints.OnDecrease.AddListener(hp => {
      ScreenFade.DamageFlash();
    });

    hitPoints.OnBecomeZero.AddListener(hp => {
      SpinsOnDeath.Begin();
      GameOverData.ResumeLevel = SceneManager.GetActiveScene().name;
      SceneEvents.Current.ChangeScene(
        GameOverScene,
        fadeOutMusic: true,
        overrideFadeOutDuration: 2f
      );
    });
  }
}
