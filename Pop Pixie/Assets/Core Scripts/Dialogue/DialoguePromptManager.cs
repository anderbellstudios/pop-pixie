using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePromptManager : MonoBehaviour {
  public bool SingletonInstance = true;
  public static DialoguePromptManager Current;

  public DialoguePromptBoxController DialoguePromptBox;

  private bool Open = false;
  private Action OnPositiveAnswer;
  private Action OnNegativeAnswer;

  void Awake() {
    if (SingletonInstance)
      Current = this;

    DialoguePromptBox.OnPositiveAnswer.AddListener(HandlePositiveAnswer);
    DialoguePromptBox.OnNegativeAnswer.AddListener(HandleNegativeAnswer);

    DialoguePromptBox.Hide();
  }

  public void Prompt(string question, string positiveAnswer, string negativeAnswer, Action onPositiveAnswer, Action onNegativeAnswer) {
    OnPositiveAnswer = onPositiveAnswer;
    OnNegativeAnswer = onNegativeAnswer;

    StateManager.AddState(State.NotPlaying);
    DialoguePromptBox.Show();
    Open = true;

    DialoguePromptBox.SetQuestion(question);
    DialoguePromptBox.SetAnswers(positiveAnswer, negativeAnswer);
  }

  void HandlePositiveAnswer() {
    Exit();
    OnPositiveAnswer();
  }

  void HandleNegativeAnswer() {
    Exit();
    OnNegativeAnswer();
  }

  void Exit() {
    DialoguePromptBox.Hide();
    Open = false;
    StateManager.RemoveState(State.NotPlaying);
  }
}
