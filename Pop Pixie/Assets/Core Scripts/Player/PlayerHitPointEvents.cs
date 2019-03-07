using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHitPointEvents : MonoBehaviour, IHitPointEvents {

  public MonoBehaviour HealthBar;
  public ScreenFade Fader;
  public float TimeToDie;

  public void Updated (HitPoints hp) {
    var hb = (HUDBar) HealthBar;
    hb.Progress = hp.Current / hp.Maximum;
  }

  public void Decreased (HitPoints hp) {
    Fader.Flash("red", 2.0f);
  }

  public void BecameZero (HitPoints hp) {
    StateManager.SetState( State.Dying );
    Fader.Fade("to black", 3.0f);
    MusicController.Current.Fade(1.0f, 0.0f, 1.0f);
    Invoke("GameOverScreen", TimeToDie);
  }

  void GameOverScreen () {
    SceneManager.LoadScene("Game Over");
  }
}
