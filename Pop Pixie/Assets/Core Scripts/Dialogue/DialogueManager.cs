using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IDialoguePageEventHandler {

  public delegate void DialogueManagerOnFinish();

  public bool SingletonInstance = true;
  public static DialogueManager Current;

  public DialogueBoxController DialogueBox;
  public SoundController SoundController;
  public float InterruptCooldown;
  public float SkipCooldown;

  private DialogueSequence Sequence;
  private int SequenceProgress;
  private bool DialogueBoxInProgress;
  private DialogueManagerOnFinish OnFinish;

  IntervalTimer SkipCooldownTimer; 
  bool ButtonReleased;

  void Awake () {
    DialogueBox.Hide();
  }

	void Start() {
    if (SingletonInstance)
      Current = this;

    SkipCooldownTimer = new IntervalTimer() {
      Interval = SkipCooldown
    };

    SkipCooldownTimer.Start();
  }
	
	// Update is called once per frame
	void Update () {
    if ( StateManager.Isnt( State.Dialogue ) )
      return;

    if ( ButtonReleased && WrappedInput.GetButton("Confirm") ) {
      ButtonReleased = false;

      if (DialogueBoxInProgress) {
        DialogueBox.FinishPage();
      } else {
        ReadNextPage();
      }
    }

    ButtonReleased = ! WrappedInput.GetButton("Confirm");

    if ( WrappedInput.GetButton("Cancel") && !DialogueBoxInProgress ) {
      ReadNextPage();
    }

    if ( WrappedInput.GetButton("Skip") && SkipCooldownTimer.Elapsed() ) {
      SkipCooldownTimer.Reset();
      ReadNextPage();
      DialogueBox.FinishPage();
    }
	}

  void ReadPage (DialoguePage page) {
    DialogueBoxInProgress = true;
    DialogueBox.Write( page.Text, this );
    DialogueBox.SetFace( page.Face );

    if ( page.HasAudioClip() ) {
      SoundController.Play( page.AudioClip );
    } else {
      SoundController.Stop();
    }
  }

  public void PageFinished () {
    DialogueBoxInProgress = false;
  }

  void ReadNextPage () {
    if ( SequenceProgress < Sequence.Pages.Count ) {
      var page = Sequence.Pages[ SequenceProgress ];
      ReadPage(page);

      SequenceProgress += 1;
    } else {
      Exit();
    }
  }

  void Exit () {
    DialogueBox.Hide();
    StateManager.SetState( State.Playing );
    OnFinish();
    SoundController.Stop();
  }

	public void Play (DialogueSequence sequence, DialogueManagerOnFinish onFinish) {
    ButtonReleased = false;
    DialogueBoxInProgress = false;
    DialogueBox.Show();
    StateManager.SetState( State.Dialogue );

    Sequence = sequence;
    SequenceProgress = 0;
    OnFinish = onFinish;

    ReadNextPage();
	}
}
