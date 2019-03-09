using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Started : MonoBehaviour, IDialogueSequenceEventHandler {

  public DialogueManager Dialogue;
  public ScreenFade Fader;
  public AudioClip Music;
  public Camera MainCamera, CutsceneCamera;
  public MentoeRunToElevator MentoeAnimation;

  private int DialogueCount;

	// Use this for initialization
	void Start () {
    Fader.Fade("from black", 2.0f);
    MusicController.Current.Play(Music, "level 1");

    MainCamera.enabled = false;
    CutsceneCamera.enabled = true;

    DialogueCount = 0;
    Dialogue.Play("Dialogue/l2d1", this);
	}

  public void SequenceFinished () {
    switch (DialogueCount) {
      case 0:
        StateManager.SetState( State.Cutscene );
        MusicController.Current.SetVolume(0.25f);
        MentoeAnimation.Run();
        Invoke("PlaySecondDialogue", 3.0f);
        break;

      case 1:
        Fader.Fade("to black", 1.0f);
        Invoke("SwitchToMainCamera", 2.0f);
        break;
    }
  }

  void PlaySecondDialogue () {
    DialogueCount = 1;
    Dialogue.Play("Dialogue/l2d2", this);
  }

  void SwitchToMainCamera () {
    MainCamera.enabled = true;
    CutsceneCamera.enabled = false;

    Fader.Fade("from black", 1.0f);
  }
}
