using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IDialogueEventHandler {

  public DialogueBoxController DialogueBox;

  private DialogueSequence Sequence;

  void ReadPage (DialoguePage page) {
    DialogueBox.Write( page.Text );
  }

  public void PageFinished () {
    Debug.Log("Finished page");
  }

	// Use this for initialization
	void Start () {
    DialogueBox.EventHandler = this;

    string json = Resources.Load<TextAsset>("Dialogue/l1d1").text;
    Sequence = DialogueSequence.ParseJSON(json);

    ReadPage( Sequence.Pages[0] );
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
