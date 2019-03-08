using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
      Fader.Fade("to black", 2.0f);
    }
  }

}
