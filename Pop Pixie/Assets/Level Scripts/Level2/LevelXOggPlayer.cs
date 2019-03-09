using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelXOggPlayer : MonoBehaviour, IDialogueSequenceEventHandler, IPromptButtonEventHandler, ILoreEventHandler {

  public LoreManager Lore;
  public DialogueManager Dialogue;
  public DialoguePromptManager PromptManager;
  public AudioSource MainMusic, SpecialMusic;

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      Dialogue.Play("Dialogue/OggPlayer", this);
    }
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
      Lore.Open("Lore/Blank", this);

      MainMusic.enabled = false;
      SpecialMusic.enabled = true;
      SpecialMusic.Play();
    }
  }

  public void Closed () {
    MainMusic.enabled = true;
    MainMusic.Play();
    SpecialMusic.enabled = false;
    SpecialMusic.Stop();
  }
}
