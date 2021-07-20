using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialoguePromptHopper : MonoBehaviour, IPromptButtonEventHandler {
  public string Question, PositiveAnswer, NegativeAnswer;

  [SerializeField] public UnityEvent OnPositiveAnswer, OnNegativeAnswer;

  public void Hop() {
    DialoguePromptManager.Current.Display(Question, PositiveAnswer, NegativeAnswer, this);
  }

  public void ButtonPressed (string button) {
    switch (button) {
      case "positive":
        OnPositiveAnswer.Invoke();
        break;

      case "negative":
        OnNegativeAnswer.Invoke();
        break;
    }
  }
}
