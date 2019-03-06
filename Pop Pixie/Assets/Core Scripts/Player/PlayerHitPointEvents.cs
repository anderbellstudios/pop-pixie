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
    Fader.Flash("red");
  }

  public void BecameZero (HitPoints hp) {
    StateManager.SetState( State.Dying );
    Fader.Fade("to black");
    Invoke("GameOverScreen", TimeToDie);
  }

  void GameOverScreen () {
    SceneManager.LoadScene("Game Over");
  }
}
