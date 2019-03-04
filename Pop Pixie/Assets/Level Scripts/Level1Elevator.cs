﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Elevator : MonoBehaviour, IDialogueSequenceEventHandler {

  public DialogueManager Dialogue;

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      StateManager.SetState( State.Dialogue );
      Dialogue.Play("Dialogue/l1d3", this);
    }
  }

  public void SequenceFinished () {
    Debug.Log("And now, elevator happens.");
  }
}