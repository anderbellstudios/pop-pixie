using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Started : MonoBehaviour, IDialogueSequenceEventHandler {

  public ScreenFade Fader;

	// Use this for initialization
	void Start () {
    Fader.Fade("from black", 2.0f);
    StateManager.SetState( State.Playing );
	}

  public void SequenceFinished () {
  }
}
