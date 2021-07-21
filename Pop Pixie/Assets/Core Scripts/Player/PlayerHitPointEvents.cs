using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PopPixie.Audio;

public class PlayerHitPointEvents : MonoBehaviour, IHitPointEvents {

  public HUDBar HealthBar;
  public ScreenFade Fader;
  public string GameOverScene = "Game Over";

  public void Updated (HitPoints hp) {
    HealthBar.Progress = hp.Current / hp.Maximum;
  }

  public void Decreased (HitPoints hp) {
    Fader.Flash("red", 2.0f);
  }

  public void BecameZero (HitPoints hp) {
    // Make sure you can't quit out to avoid dying
    SaveGame.ReadSave();
    SaveGame.WriteAutoSave();

    StateManager.SetState( State.Dying );
    Fader.Fade("to black", 2.0f);
    AudioMixer.Current.FadeOut(1.0f);
    Invoke("GameOverScreen", 2.0f);
  }

  void GameOverScreen () {
    SceneManager.LoadScene(GameOverScene);
  }
}
