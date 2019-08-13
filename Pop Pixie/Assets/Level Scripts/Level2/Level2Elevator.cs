using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Elevator : MonoBehaviour, IPromptButtonEventHandler {

  public DialogueManager Dialogue;
  public DialoguePromptManager PromptManager;
  public ScreenFade Fader;

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      PromptManager.Display(
        "(This elevator can take you to the third floor.)\n(Will you advance?)",
        "Advance",
        "Do not",
        this
      );
    }
  }

  public void ButtonPressed (string button) {
    if ( button == "positive" ) {
      StateManager.SetState( State.LoadingLevel );
      MusicController.Current.SetVolume(0.25f);
      Fader.Fade("to black", 2.0f);
      Invoke("NextLevel", 3.0f);
    }
  }

  void NextLevel () {
    SceneManager.LoadScene("Elevator");
  }
}
