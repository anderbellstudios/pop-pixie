using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHintDialogue : MonoBehaviour {

  public DialogueManager Dialogue;
  public FireableScheduler LaserScheduler;

  private bool ShouldPlayDialogue = true;

  void Start() {
    LaserScheduler.FireableRemoved += DoNotPlayDialogueAgain;
    LaserScheduler.CycleFinished   += DialogueOpportunity;
  }

  void DoNotPlayDialogueAgain() {
    ShouldPlayDialogue = false;
  }

  void DialogueOpportunity() {
    if ( ShouldPlayDialogue ) Invoke("PlayDialogue", 0.5f);
  }

  void PlayDialogue() {
    // Dialogue.Play("Dialogue/Level 3 Laser Hint", this);
    DoNotPlayDialogueAgain();
  }

}
