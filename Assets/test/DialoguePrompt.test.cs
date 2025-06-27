#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class DialoguePromptTest : ABaseTest {
  private string LastAnswer = "Pending";

  private IEnumerator Setup() {
    yield return CommonSetup();
    LoadSceneNotInBuildSettings("Assets/Unity/Scenes/Test Level.unity");
    yield return AwaitSceneChange("Test Level");

    DialoguePromptManager.Current.Prompt(
      question: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam in molestie augue, a aliquet metus. Sed ac bibendum lectus.",
      positiveAnswer: "Positive",
      negativeAnswer: "Negative",
      onPositiveAnswer: () => LastAnswer = "Positive",
      onNegativeAnswer: () => LastAnswer = "Negative"
    );

    LastAnswer = "Pending";
  }

  [UnityTest, Retry(3)]
  public IEnumerator ClickPositiveAnswer() {
    yield return Setup();
    yield return ClickByText("Positive");
    Assert.AreEqual("Positive", LastAnswer);
    AssertClosed();
  }

  [UnityTest, Retry(3)]
  public IEnumerator ClickNegativeAnswer() {
    yield return Setup();
    yield return ClickByText("Negative");
    Assert.AreEqual("Negative", LastAnswer);
    AssertClosed();
  }

  [UnityTest, Retry(3)]
  public IEnumerator ConfirmPositiveAnswer() {
    yield return Setup();
    yield return PressButton("Confirm");
    Assert.AreEqual("Positive", LastAnswer);
    AssertClosed();
  }

  [UnityTest, Retry(3)]
  public IEnumerator ConfirmNegativeAnswer() {
    yield return Setup();
    yield return MoveDownAndWait();
    yield return PressButton("Confirm");
    Assert.AreEqual("Negative", LastAnswer);
    AssertClosed();
  }

  [UnityTest, Retry(3)]
  public IEnumerator HoverAndConfirmPositiveAnswer() {
    yield return Setup();
    yield return HoverByText("Negative");
    yield return HoverByText("Positive");
    yield return PressButton("Confirm");
    Assert.AreEqual("Positive", LastAnswer);
    AssertClosed();
  }

  [UnityTest, Retry(3)]
  public IEnumerator HoverAndConfirmNegativeAnswer() {
    yield return Setup();
    yield return HoverByText("Positive");
    yield return HoverByText("Negative");
    yield return PressButton("Confirm");
    Assert.AreEqual("Negative", LastAnswer);
    AssertClosed();
  }

  [UnityTest, Retry(3)]
  public IEnumerator PercyScreenshots() {
    yield return Setup();
    yield return TakePercyScreenshot("DialoguePrompt");
  }

  private void AssertClosed() {
    Assert.IsFalse(
      DialoguePromptManager
        .Current
        .DialoguePromptBox
        .gameObject
        .activeSelf
    );
  }
}
#endif
