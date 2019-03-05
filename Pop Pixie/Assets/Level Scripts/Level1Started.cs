using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Started : MonoBehaviour, IDialogueSequenceEventHandler {

  public DialogueManager Dialogue;

	// Use this for initialization
	void Start () {
    Dialogue.Play("Dialogue/l1d1", this);
	}

  public void SequenceFinished () {
  }
}
