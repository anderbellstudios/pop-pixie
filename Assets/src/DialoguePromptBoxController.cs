using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialoguePromptBoxController : MonoBehaviour {
  public TMP_Text Question, PositiveAnswerText, NegativeAnswerText;
  public Button PositiveAnswerButton, NegativeAnswerButton;

  public UnityEvent OnPositiveAnswer
    => PositiveAnswerButton.onClick;

  public UnityEvent OnNegativeAnswer
    => NegativeAnswerButton.onClick;

  public void SetQuestion(string text) {
    Question.text = text;
  }

  public void SetAnswers(string positiveAnswer, string negativeAnswer) {
    PositiveAnswerText.text = positiveAnswer;
    NegativeAnswerText.text = negativeAnswer;
  }

  public void Show() {
    gameObject.SetActive(true);

    PositiveAnswerButton.Select();
    PositiveAnswerButton.OnSelect(null);
  }

  public void Hide() {
    gameObject.SetActive(false);
  }
}
