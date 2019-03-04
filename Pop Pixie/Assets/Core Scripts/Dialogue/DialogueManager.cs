using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IDialogueEventHandler {

  public DialogueBoxController DialogueBox;
  public float InterruptCooldown;

  private DialogueSequence Sequence;
  private int SequenceProgress;
  private bool DialogueBoxInProgress;

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
    }
  }

	// Use this for initialization
	void Start () {
    DialogueBox.EventHandler = this;
    DialogueBoxInProgress = false;

    string json = Resources.Load<TextAsset>("Dialogue/l1d1").text;
    Sequence = DialogueSequence.ParseJSON(json);
    SequenceProgress = 0;

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
