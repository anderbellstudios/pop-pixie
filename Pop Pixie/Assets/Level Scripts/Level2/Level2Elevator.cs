using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Elevator : MonoBehaviour, IDialogueSequenceEventHandler, IPromptButtonEventHandler {

  public DialogueManager Dialogue;
  public DialoguePromptManager PromptManager;
  public ScreenFade Fader;

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      Dialogue.Play("Dialogue/l2d3", this);
    }
  }

  public void SequenceFinished () {
    ShowPrompt();
  }

  void ShowPrompt () {
    PromptManager.Display(
      "(It appears that your journey must end here for now.)\n(Will you depart?)",
      "Depart",
      "Not yet",
      this
    );
  }

  public void ButtonPressed (string button) {
    if ( button == "positive" ) {
      StateManager.SetState( State.LoadingLevel );
      Fader.Fade("to black", 2.0f);
      MusicController.Current.Fade(0.25f, 0.0f, 2.0f);
      Invoke("EndOfGameScene", 2.5f);
    }
  }

  void EndOfGameScene () {
    SceneManager.LoadScene("End of Game");
  }

}
