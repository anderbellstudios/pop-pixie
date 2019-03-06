using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Started : MonoBehaviour, IDialogueSequenceEventHandler {

  public DialogueManager Dialogue;
  public ScreenFade Fader;

	// Use this for initialization
	void Start () {
    Fader.Fade("from black", 2.0f);
    Dialogue.Play("Dialogue/l1d1", this);
	}

  public void SequenceFinished () {
  }
}
