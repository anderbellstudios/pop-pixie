using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Started : MonoBehaviour {

  public DialogueManager Dialogue;
  public ScreenFade Fader;

	// Use this for initialization
	void Start () {
    Fader.Fade("from black", 2.0f);
    StateManager.SetState( State.Playing );
    GDCall.UnlessLoad( PlayDialogue );
  }

  public void PlayDialogue() {
    // Dialogue.Play("Dialogue/l1d1", this);
	}
}
