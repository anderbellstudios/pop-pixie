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

  private DialogueSequence DialogueSequence;
  private int CurrentPage;
  private bool Open = false;
  private Action OnFinish;

  void Awake () {
    if (SingletonInstance)
      Current = this;

    DialogueBox.Hide();
  }

	public void Play(DialogueSequence dialogueSequence, Action onFinish) {
    DialogueSequence = dialogueSequence;
    CurrentPage = -1;
    OnFinish = onFinish;

    StateManager.AddState(State.NotPlaying);
    DialogueBox.Show();
    Open = true;

    NextPage();
	}
	
	void Update () {
    if (!Open)
      return;

    if (WrappedInput.GetButtonUp("confirm")) {
      if (DialogueBox.TypewriterActive) {
        DialogueBox.SkipTypewriter();
      } else {
        NextPage();
      }
    }

    if (WrappedInput.GetButtonUp("cancel")) {
      Exit();
    }
	}

  void NextPage() {
    CurrentPage += 1;

    if (CurrentPage >= DialogueSequence.PageCount) {
      Exit();
    } else {
      ShowPage(DialogueSequence.GetPage(CurrentPage));
    }
  }

  void ShowPage(DialoguePage page) {
    DialogueBox.SetHeading(page.Speaker);
    DialogueBox.SetFace(page.Face);

    float speed = TypewriterSpeed * page.RelativeSpeed;
    DialoguePreprocessor preprocessor = new DialoguePreprocessor(speed);
    DialogueBox.WriteBody(preprocessor.Preprocess(page.Text), speed);

    if (page.HasAudioClip()) {
      SoundController.Play(page.AudioClip);
    } else {
      SoundController.Stop();
    }
  }

  void Exit() {
    SoundController.Stop();
    DialogueBox.Hide();
    Open = false;
    StateManager.RemoveState(State.NotPlaying);
    OnFinish();
  }
}
