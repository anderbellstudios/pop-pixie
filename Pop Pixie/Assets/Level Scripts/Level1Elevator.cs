using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Elevator : MonoBehaviour, IDialogueSequenceEventHandler, IPromptButtonEventHandler {

  public DialogueManager Dialogue;
  public DialoguePromptManager PromptManager;
  public bool Triggered;

  void OnTriggerEnter2D (Collider2D other) {
    if ( Triggered ) {
      ShowPrompt();
      return;
    }

    if ( other.tag == "Player" ) {
      Dialogue.Play("Dialogue/l1d3", this);
    }
  }

  public void SequenceFinished () {
    Triggered = true;
    ShowPrompt();
  }

  void ShowPrompt () {
    PromptManager.Display(
      "(This elevator can take you to the second floor.)\n(Will you advance?)",
      "Advance",
      "Do not",
      this
    );
  }

  public void ButtonPressed (string button) {
    if ( button == "positive" ) {
      GameDataController.Current.NextLevel();
    }
  }
}
