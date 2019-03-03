using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IDialogueEventHandler {

  public DialogueBoxController DialogueBox;

  private DialogueSequence Sequence;
  private int SequenceProgress;

  void ReadPage (DialoguePage page) {
    DialogueBox.Write( page.Text );
  }

  public void PageFinished () {
    ReadNextPage();
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

    string json = Resources.Load<TextAsset>("Dialogue/l1d1").text;
    Sequence = DialogueSequence.ParseJSON(json);
    SequenceProgress = 0;

    ReadNextPage();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
