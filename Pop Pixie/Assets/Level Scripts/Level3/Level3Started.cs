using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Started : MonoBehaviour, IDialogueSequenceEventHandler {

  public DialogueManager Dialogue;
  public ScreenFade Fader;
  // public AudioClip Music;
  public CameraPan Pan1;
  public CameraPan Pan2;
  public float DelayBeforePan1;
  public float DelayBeforePan2;
  public LaserScheduler LaserScheduler;

	// Use this for initialization
	void Start () {
    StateManager.SetState( State.Cutscene );
    Fader.Fade("from black", 2.0f);
    // MusicController.Current.Play(Music, "level 1");
    Invoke("StartPan1", DelayBeforePan1);
	}

  void StartPan1() {
    Pan1.Perform(this, "Pan1Finished");
  }

  void Pan1Finished() {
    Invoke("StartPan2", DelayBeforePan2);
  }

  void StartPan2() {
    Pan2.Perform(this, "Pan2Finished");
  }

  void Pan2Finished() {
    StateManager.SetState( State.Playing );
    // LaserScheduler.enabled = true;
  }

  public void SequenceFinished () {
  }

}
