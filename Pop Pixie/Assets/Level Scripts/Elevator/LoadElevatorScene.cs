using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadElevatorScene : MonoBehaviour, IDialogueSequenceEventHandler, IPromptButtonEventHandler {

  public DialogueManager Dialogue;
  public DialoguePromptManager PromptManager;
  public ScreenFade Fader;

  public string FlavourFloorName;
  public string NextLevel;
  public string PreElevatorDialogue = "";

  public bool Triggered;

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {

      if ( ShouldPlayDialogue() ) {
        Dialogue.Play("Dialogue/l1d3", this);
      } else {
        ShowPrompt();
      }

    }
  }

  bool ShouldPlayDialogue() {
    return !Triggered && PreElevatorDialogue != "";
  }

  public void SequenceFinished () {
    Triggered = true;
    ShowPrompt();
  }

  void ShowPrompt () {
    PromptManager.Display(
      "(This elevator can take you to the " + FlavourFloorName + " floor.)\n(Will you advance?)",
      "Advance",
      "Do not",
      this
    );
  }

  public void ButtonPressed (string button) {
    if ( button == "positive" ) {
      StateManager.SetState( State.LoadingLevel );
      MusicController.Current.SetVolume(0.25f);
      Fader.Fade("to black", 2.0f);
      Invoke("LoadScene", 3.0f);
    }
  }

  void LoadScene () {
    ElevatorData.NextLevel = NextLevel;
    SceneManager.LoadScene("Elevator");
  }
}
