using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2OggPlayer : AInspectable, IDialogueSequenceEventHandler, IPromptButtonEventHandler {

  public LoreItem LoreItem;
  public DialogueManager Dialogue;
  public DialoguePromptManager PromptManager;
  public SongHopper SongHopper;

  public override void OnInspect() {
    Dialogue.Play("Dialogue/OggPlayer", this);
  }

  public void SequenceFinished () {
    ShowPrompt();
  }

  void ShowPrompt () {
    PromptManager.Display(
      "(It appears to be stuck playing the same track on repeat.)\n(Do you listen to it?)",
      "Listen",
      "Do not",
      this
    );
  }

  public void ButtonPressed (string button) {
    if ( button == "positive" ) {
      LoreManager.Current.Open(LoreItem, () => {
        SongHopper.Stop();
      });

      SongHopper.Hop();
    }
  }
}
