using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Started : MonoBehaviour, IDialogueSequenceEventHandler {

  public DialogueManager Dialogue;
  public ScreenFade Fader;
  public AudioClip Music;

	// Use this for initialization
	void Start () {
    Fader.Fade("from black", 2.0f);
    StateManager.SetState( State.Playing );
    MusicController.Current.Play(Music, "level 1");
    GDCall.UnlessLoad( PlayDialogue );
  }

  public void PlayDialogue() {
    Dialogue.Play("Dialogue/l1d1", this);
	}

  public void SequenceFinished () {
  }
}
