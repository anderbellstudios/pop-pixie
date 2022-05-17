using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Started : MonoBehaviour {

  public DialogueManager Dialogue;
  public ScreenFade Fader;
  public Camera MainCamera, CutsceneCamera;
  public MentoeRunToElevator MentoeAnimation;

  private int DialogueCount;

	// Use this for initialization
	void Start () {
    Fader.Fade("from black", 2.0f);
    // StateManager.SetState( State.Playing );

    Debug.Log("Not implemented");

    GDCall.UnlessLoad( StartCutscene );
    GDCall.IfLoad( MentoeAnimation.Skip );
  }

  public void StartCutscene() {
    MainCamera.enabled = false;
    CutsceneCamera.enabled = true;

    DialogueCount = 0;
    // Dialogue.Play("Dialogue/l2d1", this);
    // StateManager.SetState( State.Cutscene );
    MentoeAnimation.Run();
    Invoke("PlaySecondDialogue", 3.0f);
	}

  void PlaySecondDialogue () {
    DialogueCount = 1;
    // Dialogue.Play("Dialogue/l2d2", this);
    Fader.Fade("to black", 0.5f);
    Invoke("SwitchToMainCamera", 0.5f);
  }

  void SwitchToMainCamera () {
    MainCamera.enabled = true;
    CutsceneCamera.enabled = false;

    Fader.Fade("from black", 0.5f);
  }
}
