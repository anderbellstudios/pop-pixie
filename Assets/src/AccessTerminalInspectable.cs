using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessTerminalInspectable : AInspectable {
  public override void OnInspect() {
    Debug.Log("You inspected!");
  }

  public override string AInspectablePromptText()
    => "Press [Inspect] to use the <color=#ffff00>access terminal</color>";

  public override string AInspectableUninspectableText()
    => "Find a <color=#ffff00>keycard</color> to use the <color=#ffff00>access terminal</color>";

  // TODO: Refactor state
  public override bool IsInspectable() => ElevatorDoor.Current.HasKeycard;
}
