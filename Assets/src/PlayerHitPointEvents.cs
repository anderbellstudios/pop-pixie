using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHitPointEvents : MonoBehaviour {
  public HitPoints OverrideHitPoints;
  public HUDBar HealthBar;
  public ScreenFade Fader;
  public string GameOverScene = "Game Over";

  void Awake() {
    HitPoints hitPoints = OverrideHitPoints ?? GetComponent<HitPoints>();

    hitPoints.OnUpdate.AddListener(hp => {
      HealthBar.Progress = hp.Current / hp.Maximum;
    });

    hitPoints.OnDecrease.AddListener(hp => {
      Fader.Flash("red", 2.0f);
    });

    hitPoints.OnBecomeZero.AddListener(hp => {
      StateManager.AddState(State.PlayerDying);
      Fader.Fade("to black", 2.0f);
      AudioFadeOut.Current.FadeOut(1.0f);
      Invoke("GameOverScreen", 2.0f);
    });
  }

  void GameOverScreen() {
    GameOverData.ResumeLevel = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene(GameOverScene);
  }
}
