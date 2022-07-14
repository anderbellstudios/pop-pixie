using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
  public bool SingletonInstance = true;
  public static DialogueManager Current;

  public DialogueBoxController DialogueBox; 
  public SoundController SoundController;
  public float TypewriterSpeed;
  public float ContinuePromptDelay;

  private DialogueSequence DialogueSequence;
  private int CurrentPageIndex;
  private DialoguePage CurrentPage;
  private bool Open = false;
  private Action OnFinish;
  private ButtonPressHelper ButtonPressHelper = new MultipleButtonPressHelper();
  private IntervalTimer ContinuePromptTimer = new IntervalTimer();

  void Awake () {
    if (SingletonInstance)
      Current = this;

    ContinuePromptTimer = new IntervalTimer() {
      Interval = ContinuePromptDelay
    };

    DialogueBox.Hide();

    DialogueBox.OnFinished.AddListener(() => ContinuePromptTimer.Reset());
  }

	public void Play(DialogueSequence dialogueSequence, Action onFinish) {
    DialogueSequence = dialogueSequence;
    CurrentPageIndex = -1;
    OnFinish = onFinish;

    StateManager.AddState(State.NotPlaying);
    DialogueBox.Show();
    Open = true;
    ButtonPressHelper.Clear();

    NextPage();
	}
	
	void Update () {
    if (!Open)
      return;

    if (ButtonPressHelper.GetButtonPress("confirm")) {
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

    DialogueBox.SetContinuePromptVisible(ContinuePromptTimer.Elapsed());
	}

  void NextPage() {
    CurrentPageIndex += 1;

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

    if (CurrentPage.HasAudioClip()) {
      SoundController.Play(CurrentPage.AudioClip);
    } else {
      SoundController.Stop();
    }

    ContinuePromptTimer.Stop();
  }

  void Exit() {
    SoundController.Stop();
    DialogueBox.Hide();
    Open = false;
    StateManager.RemoveState(State.NotPlaying);
    OnFinish();
  }
}
