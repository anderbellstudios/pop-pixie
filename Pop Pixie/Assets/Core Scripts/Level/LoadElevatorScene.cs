using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PopPixie.Audio;

public class LoadElevatorScene : AInspectable {
  public ScreenFade Fader;

  public string FlavourFloorName;
  public string NextLevel;
  public string PreElevatorDialogue = "";

  public bool Triggered;

  public override void OnInspect() {
    if ( ShouldPlayDialogue() ) {
      // Dialogue.Play("Dialogue/l1d3", this);
      Triggered = true;
      ShowPrompt();
    } else {
      ShowPrompt();
    }
  }

  bool ShouldPlayDialogue() {
    return !Triggered && PreElevatorDialogue != "";
  }

  void ShowPrompt () {
    DialoguePromptManager.Current.Prompt(
      "(This elevator can take you to the " + FlavourFloorName + " floor.)\n(Will you advance?)",
      "Advance",
      "Do not",
      () => {
        StateManager.SetState( State.LoadingLevel );
        Fader.Fade("to black", 2.0f);
        AudioMixer.Current.FadeOut(2.0f);
        Invoke("LoadScene", 3.0f);
      },
      () => {}
    );
  }

  void LoadScene () {
    ElevatorData.NextLevel = NextLevel;
    SceneManager.LoadScene("Elevator");
  }
}
