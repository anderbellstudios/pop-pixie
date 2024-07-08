using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AccessTerminalSprite : AInspectable {
  public AccessTerminalConfig Config;
  public UnityEvent OnAccess;

  public override void OnInspect() {
    OnAccess.Invoke();

    LevelObjectives.Current.UsedAccessTerminal = true;
    LoreItemData.RecordRead(Config.LoreItem);
    StateManager.AddState(State.NotPlaying);

    AccessTerminalManager.Current.Open(Config, () => {
      LoreManager.Current.Open(Config.LoreItem, () => {
        StateManager.RemoveState(State.NotPlaying);
      });
    });
  }

  public override string AInspectablePromptText()
    => "Press [Inspect] to use the <color=#ffff00>Access Terminal</color>";

  public override string AInspectableUninspectableText()
    => LevelObjectives.Current.UsedAccessTerminal ? null : "Find a <color=#ffff00>Keycard</color> to use the <color=#ffff00>Access Terminal</color>";

  public override bool IsInspectable()
    => LevelObjectives.Current.GotKeycard && !LevelObjectives.Current.UsedAccessTerminal;
}
