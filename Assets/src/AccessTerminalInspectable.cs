using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessTerminalInspectable : AInspectable {
  public AccessTerminalConfig Config;

  private bool HasBeenAccessed = false;

  public override void OnInspect() {
    HasBeenAccessed = true;
    LoreItemData.RecordRead(Config.LoreItem);
    StateManager.AddState(State.NotPlaying);

    AccessTerminalManager.Current.Open(Config, () => {
      LoreManager.Current.Open(Config.LoreItem, () => {
        StateManager.RemoveState(State.NotPlaying);
      });
    });
  }

  public override string AInspectablePromptText()
    => "Press [Inspect] to use the <color=#ffff00>access terminal</color>";

  public override string AInspectableUninspectableText()
    => HasBeenAccessed ? null : "Find a <color=#ffff00>keycard</color> to use the <color=#ffff00>access terminal</color>";

  // TODO: Refactor state
  public override bool IsInspectable()
    => ElevatorDoor.Current.HasKeycard && !HasBeenAccessed;
}
