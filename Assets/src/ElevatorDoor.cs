using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : AInspectable {
  public bool SingletonInstance = true;
  public static ElevatorDoor Current;

  public string NextLevel;
  public SceneChangeHopper SceneChangeHopper;

  void Awake() {
    if (SingletonInstance)
      Current = this;
  }

  public override bool IsInspectable() => LevelObjectives.Current.UsedAccessTerminal;

  public override String AInspectablePromptText()
    => "Press [Inspect] to use the <color=#ffff00>elevator</color>";

  public override String AInspectableUninspectableText()
    => "Use an <color=#ffff00>Access Terminal</color> to gain access to higher floors";

  public override void OnInspect() {
    DialoguePromptManager.Current.Prompt(
      "Advance to the next floor?",
      "Advance",
      "Do not",
      () => {
        ElevatorData.NextLevel = NextLevel;
        SceneChangeHopper.Hop();
      },
      () => { }
    );
  }
}
