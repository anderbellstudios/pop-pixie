using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePromptManager : MonoBehaviour, IDialoguePageEventHandler {

  public DialogueBoxController DialogueBox;
  public PromptButtonController PromptButtons;
  public Sprite PromptFace;

  public void Display () {
    StateManager.SetState( State.DialoguePrompt );
    DialogueBox.Show();
    DialogueBox.Write("(This is a question?)", this);
    DialogueBox.SetFace( PromptFace );

    PromptButtons.Write("Hello", "World");
    PromptButtons.Hide();
  }

  public void PageFinished () {
    PromptButtons.Show();
  }

  void Exit () {
    PromptButtons.Hide();
    DialogueBox.Hide();
    StateManager.SetState( State.Playing );
  }

  void Awake () {
    PromptButtons.Hide();
  }
	
	// Update is called once per frame
	void Update () {
    if ( StateManager.Isnt( State.DialoguePrompt ) )
      return;

    if ( Input.GetButton("AbortDialogue") ) {
      Exit();
    }
	}
}
