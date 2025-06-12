using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AccessTerminalSprite : AInspectable {
  public AccessTerminalConfig Config;
  public int LevelIndex = -1;
  public UnityEvent OnAccess;

  void Start() {
    // Handle the case where it's already been used
    if (LevelObjectives.UsedAccessTerminal) {
      OnAccess.Invoke();
    }

    AInspectableStart();
  }

  public override void OnInspect() {
    OnAccess.Invoke();

    LevelObjectives.UsedAccessTerminal = true;

    if (Config.LoreItem != null)
      LoreItemData.RecordRead(Config.LoreItem);

    if (LevelIndex > -1)
      LevelCompletionData.CompleteLevel(LevelIndex);

    StateManager.AddState(State.NotPlayingContinueSounds);

    AccessTerminalManager.Current.Open(Config, () => {
      if (Config.LoreItem == null) {
        StateManager.RemoveState(State.NotPlayingContinueSounds);
      } else {
        LoreManager.Current.Open(Config.LoreItem, () => {
          StateManager.RemoveState(State.NotPlayingContinueSounds);
        });
      }
    });
  }

  public override string AInspectablePromptText()
    => "Press [Inspect] to use the <color=#ffff00>Access Terminal</color>";

  public override string AInspectableUninspectableText()
    => LevelObjectives.UsedAccessTerminal ? null : "Find a <color=#ffff00>Keycard</color> to use the <color=#ffff00>Access Terminal</color>";

  public override bool IsInspectable()
    => LevelObjectives.GotKeycard && !LevelObjectives.UsedAccessTerminal;
}
