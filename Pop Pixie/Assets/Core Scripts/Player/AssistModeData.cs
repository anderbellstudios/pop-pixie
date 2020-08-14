using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistModeData {

  public enum AssistModeHP: int {
    Normal = 1,
    X2     = 2,
    X4     = 4,
    Max    = 1000000
  }

  public static EnumeratorButton<AssistModeHP> MakeAssistModeButton(Action<String> SetLabel) {
    return new EnumeratorButton<AssistModeHP>(
      new List<AssistModeHP>() {
        AssistModeHP.Normal,
        AssistModeHP.X2,
        AssistModeHP.X4,
        AssistModeHP.Max
      },
      HPAdjustment,
      (hp) => {
        HPAdjustment = hp;
        SetLabel.Invoke( AssistModeHPToLabel(hp) );
      }
    );
  }

  public static AssistModeHP HPAdjustment {
    get {
      return ConfigData.Current.Fetch("assist-mode-hp-adjustment", orSetEqualTo: AssistModeHP.Normal);
    }

    set {
      ConfigData.Current.Set("assist-mode-hp-adjustment", value);
    }
  }

  private static string AssistModeHPToLabel(AssistModeHP hp) {
    switch (hp) {
      case AssistModeHP.X2:
        return "Assist: 2x HP";
      case AssistModeHP.X4:
        return "Assist: 4x HP";
      case AssistModeHP.Max:
        return "Assist: Max HP";
      case AssistModeHP.Normal:
      default:
        return "Assist: None";
    }
  }

}
