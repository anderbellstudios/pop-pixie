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
    DialogueBoxInProgress = true;
    DialogueBox.Write( page.Text );
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
      EventHandler.SequenceFinished();
      DialogueBox.Hide();
    }
  }

	public void Play (string sequence_name, IDialogueSequenceEventHandler event_handler) {
    DialogueBox.EventHandler = this;
    DialogueBoxInProgress = false;
    DialogueBox.Show();

    string json = Resources.Load<TextAsset>(sequence_name).text;
    Sequence = DialogueSequence.ParseJSON(json);
    SequenceProgress = 0;

    EventHandler = event_handler;

    ReadNextPage();
	}

  private bool ButtonDown;
	
	// Update is called once per frame
	void Update () {
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
	}
}
