using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Started : MonoBehaviour, IDialogueSequenceEventHandler {

  public ScreenFade Fader;
  public AudioClip Music;

	// Use this for initialization
	void Start () {
    Fader.Fade("from black", 2.0f);
    MusicController.Current.Play(Music, "level 1");
    StateManager.SetState( State.Playing );
	}

  public void SequenceFinished () {
  }
}
