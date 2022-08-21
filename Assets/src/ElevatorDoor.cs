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

  private bool HasKeycard = false;
  public void GotKeycard() => HasKeycard = true;

  void Awake() {
    if (SingletonInstance)
      Current = this;
  }

  void Start() {
    InGamePrompt.Current.RegisterSource(100, () =>
      (_Nearby && !HasKeycard)
      ? "Find a <color=#ffff00>keycard</color> to use the <color=#ffff00>elevator</color>"
      : null
    );

    AInspectableStart();
  }

  public override bool IsInspectable() => HasKeycard;

  public override String AInspectablePromptText()
    => "Press [Inspect] to use the <color=#ffff00>elevator</color>";

  public override void OnInspect() {
    DialoguePromptManager.Current.Prompt(
      "Advance to the next floor?",
      "Advance",
      "Do not",
      () => {
        ElevatorData.NextLevel = NextLevel;
        SceneChangeHopper.Hop();
      },
      () => {}
    );
  }
}
