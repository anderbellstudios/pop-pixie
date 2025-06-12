using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanelMenu : AMenu {
  protected override void LocalGainedFocus() {
    FirstSelected = GetActiveButtons().Find(button => {
      int nextLevel = LevelCompletionData.LevelCompleted + 1;
      int buttonLevel = button.GetComponent<ElevatorPanelButton>().LevelIndex;
      return buttonLevel == nextLevel;
    });
  }
}
