using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Started : MonoBehaviour, IDialogueSequenceEventHandler {

  public DialogueManager Dialogue;

  public void SequenceFinished () {
    StateManager.SetState( State.Playing );
  }

	// Use this for initialization
	void Start () {
    StateManager.SetState( State.Dialogue );
    Dialogue.Play("Dialogue/l1d1", this);
	}
}
