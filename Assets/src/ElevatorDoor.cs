using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : AInspectable, ISerializableComponent {
  public string[] SerializableFields { get; } = { "HasKeycard" };

  public bool SingletonInstance = true;
  public static ElevatorDoor Current;

  public string NextLevel;
  public SceneChangeHopper SceneChangeHopper;

  public bool HasKeycard = false;
  public void GotKeycard() => HasKeycard = true;

  void Awake() {
    if (SingletonInstance)
      Current = this;
  }

  public override bool IsInspectable() => HasKeycard;

  public override String AInspectablePromptText()
    => "Press [Inspect] to use the <color=#ffff00>elevator</color>";

  public override String AInspectableUninspectableText()
    => "Find a <color=#ffff00>keycard</color> to use the <color=#ffff00>elevator</color>";

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
