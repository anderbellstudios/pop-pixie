using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Started : MonoBehaviour, IDialogueSequenceEventHandler {

  public DialogueManager Dialogue;
  public ScreenFade Fader;
  public AudioClip Music;

  private int DialogueCount;

	// Use this for initialization
	void Start () {
    Fader.Fade("from black", 2.0f);
    MusicController.Current.Play(Music, "level 1");
    DialogueCount = 0;
    Dialogue.Play("Dialogue/l2d1", this);
	}

  public void SequenceFinished () {
    switch (DialogueCount) {
      case 0:
        DoMentoeRun();
        Invoke("PlaySecondDialogue", 3.0f);
        break;
    }
  }

  void DoMentoeRun () {
    StateManager.SetState( State.Cutscene );
  }

  void PlaySecondDialogue () {
    DialogueCount = 1;
    Dialogue.Play("Dialogue/l2d2", this);
  }
}
