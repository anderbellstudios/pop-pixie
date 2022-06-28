using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialoguePromptHopper : MonoBehaviour {
  public string Question, PositiveAnswer, NegativeAnswer;

  [SerializeField] public UnityEvent OnPositiveAnswer, OnNegativeAnswer;

  public void Hop() {
    DialoguePromptManager.Current.Prompt(
      Question,
      PositiveAnswer,
      NegativeAnswer,
      () => { OnPositiveAnswer.Invoke(); },
      () => { OnNegativeAnswer.Invoke(); }
    );
  }
}
