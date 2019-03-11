using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IDialoguePageEventHandler {

  public DialogueBoxController DialogueBox;
  public float InterruptCooldown;

  private DialogueSequence Sequence;
  private int SequenceProgress;
  private bool DialogueBoxInProgress;
  private IDialogueSequenceEventHandler EventHandler;

  void ReadPage (DialoguePage page) {
    Debug.Log(page.HasVoiceLine());
    DialogueBoxInProgress = true;
    DialogueBox.Write( page.Text, this );
    DialogueBox.SetFace( page.Face() );
  }

  public void PageFinished () {
    DialogueBoxInProgress = false;
  }

  void ReadNextPage () {
    if ( SequenceProgress < Sequence.Pages.Length ) {
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
    MusicController.Current.SetVolume(1.0f);
    EventHandler.SequenceFinished();
  }

	public void Play (string sequence_name, IDialogueSequenceEventHandler event_handler) {
    DialogueBoxInProgress = false;
    DialogueBox.Show();
    StateManager.SetState( State.Dialogue );
    MusicController.Current.SetVolume(0.25f);

    string json = Resources.Load<TextAsset>(sequence_name).text;
    Sequence = DialogueSequence.ParseJSON(json);
    SequenceProgress = 0;

    EventHandler = event_handler;

    ReadNextPage();
	}

  private bool ButtonDown;

  void Awake () {
    DialogueBox.Hide();
  }
	
	// Update is called once per frame
	void Update () {
    if ( StateManager.Isnt( State.Dialogue ) )
      return;

    if ( Input.GetButton("Submit") ) {
      if ( ButtonDown != true ) {
        ButtonDown = true;

        if (DialogueBoxInProgress) {
          DialogueBox.FinishPage();
        } else {
          ReadNextPage();
        }
      }
    } else {
      ButtonDown = false;
    }

    if ( Input.GetButton("AbortDialogue") ) {
      Exit();
    }
	}
}
