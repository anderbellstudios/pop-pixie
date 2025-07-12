using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
  public bool SingletonInstance = true;
  public static DialogueManager Current;

  public DialogueBoxController DialogueBox;
  public PlaySound PlaySound;
  public float TypewriterSpeed;
  public float ContinuePromptDelay;

  private DialogueSequence DialogueSequence;
  private int CurrentPageIndex;
  private DialoguePage CurrentPage;
  private bool Open = false;
  private Action OnFinish;
  private ButtonPressHelper ButtonPressHelper = new MultipleButtonPressHelper();
  private AsyncTimer.EnqueuedEvent ContinuePromptTimer;

  void Awake() {
    if (SingletonInstance)
      Current = this;

    DialogueBox.Hide();

    DialogueBox.OnFinished.AddListener(() => {
      ContinuePromptTimer = AsyncTimer.BaseTime.SetTimeout(
        () => {
          DialogueBox.SetContinuePromptVisible(true);
        },
        ContinuePromptDelay
      );
    });
  }

  public void Play(DialogueSequence dialogueSequence, Action onFinish) {
    DialogueSequence = dialogueSequence;
    CurrentPageIndex = -1;
    OnFinish = onFinish;

    StateManager.AddState(State.NotPlaying);
    DialogueBox.Show();
    Open = true;
    ButtonPressHelper.Clear();
    dialogueSequence.DialogueMusicFadeBehaviour.ApplyEnterBehaviour();

    NextPage();
  }

  void Update() {
    if (!Open)
      return;

    if (ButtonPressHelper.GetButtonPress("confirm") || WrappedInput.GetButtonUp("Click")) {
      if (DialogueBox.TypewriterActive) {
        if (DialogueSeenBeforeData.GetSeenBefore(CurrentPage))
          DialogueBox.SkipTypewriter();
      } else {
        DialogueSeenBeforeData.SetSeenBefore(CurrentPage);
        NextPage();
      }
    }

    if (Debug.isDebugBuild && ButtonPressHelper.GetButtonPress("cancel")) {
      Exit();
    }
  }

  void NextPage() {
    CurrentPageIndex += 1;

    DialogueBox.SetContinuePromptVisible(false);
    AsyncTimer.BaseTime.ClearTimeout(ContinuePromptTimer);

    if (CurrentPageIndex >= DialogueSequence.PageCount) {
      Exit();
    } else {
      CurrentPage = DialogueSequence.GetPage(CurrentPageIndex);
      ShowCurrentPage();
    }
  }

  void ShowCurrentPage() {
    DialogueBox.SetHeading(CurrentPage.Speaker);
    DialogueBox.SetFace(CurrentPage.Face);

    DialoguePreprocessor preprocessor = new DialoguePreprocessor(TypewriterSpeed);
    DialogueBox.WriteBody(preprocessor.Preprocess(CurrentPage.Text), TypewriterSpeed);

    PlaySound.Stop();
    if (CurrentPage.HasAudioClip()) {
      PlaySound.Play(CurrentPage.VoiceLineKey);
    }

    if (CurrentPage.ShouldAutoAdvance()) {
      DialoguePage autoAdvancePage = CurrentPage;

      AsyncTimer.BaseTime.SetTimeout(
        () => {
          if (CurrentPage == autoAdvancePage) {
            NextPage();
          }
        },
        CurrentPage.AutoAdvanceDelay
      );
    }
  }

  void Exit() {
    PlaySound.Stop();
    DialogueBox.Hide();
    Open = false;
    StateManager.RemoveState(State.NotPlaying);
    DialogueSequence.DialogueMusicFadeBehaviour.ApplyExitBehaviour();
    OnFinish();
  }
}
