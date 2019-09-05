using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePromptManager : MonoBehaviour, IDialoguePageEventHandler, IPromptButtonEventHandler {

  public DialogueBoxController DialogueBox;
  public PromptButtonController PromptButtons;
  public Sprite PromptFace;

  private IPromptButtonEventHandler EventHandler;

  public void Display (string question, string pveAns, string nveAns, IPromptButtonEventHandler event_handler) {
    StateManager.SetState( State.DialoguePrompt );
    MusicController.Current.SetVolume(0.25f);

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
    MusicController.Current.SetVolume(1.0f);
  }

  void Awake () {
    PromptButtons.Hide();
  }

}
