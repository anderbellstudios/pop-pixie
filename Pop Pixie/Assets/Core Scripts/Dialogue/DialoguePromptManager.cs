using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePromptManager : MonoBehaviour, IDialoguePageEventHandler, IPromptButtonEventHandler {

  public bool SingletonInstance = true;
  public static DialoguePromptManager Current;

  public DialogueBoxController DialogueBox;
  public PromptButtonController PromptButtons;
  public Sprite PromptFace;

  private IPromptButtonEventHandler EventHandler;

  void Awake () {
    if (SingletonInstance)
      Current = this;

    PromptButtons.Hide();
  }

  public void Display (string question, string pveAns, string nveAns, IPromptButtonEventHandler event_handler) {
    StateManager.SetState( State.DialoguePrompt );

    EventHandler = event_handler;
    DialogueBox.Show();
    DialogueBox.Write(question, this);
    DialogueBox.SetFace( PromptFace );

    PromptButtons.Write(pveAns, nveAns, this);
    PromptButtons.Hide();
  }

  public void PageFinished () {
    PromptButtons.Show();
  }

  public void ButtonPressed (string button) {
    Exit();
    EventHandler.ButtonPressed(button);
  }

  void Exit () {
    PromptButtons.Hide();
    DialogueBox.Hide();
    StateManager.SetState( State.Playing );
  }

}
