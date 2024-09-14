using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelObjectives {
  public static bool GotKeycard {
    get { return ActivatedData.IsActivated(GotKeycardKey); }

    set {
      if (value) {
        ActivatedData.RecordActivation(GotKeycardKey);
      } else {
        throw new System.Exception("Cannot set GotKeycard to false");
      }
    }
  }

  public static bool UsedAccessTerminal {
    get { return ActivatedData.IsActivated(UsedAccessTerminalKey); }

    set {
      if (value) {
        ActivatedData.RecordActivation(UsedAccessTerminalKey);
      } else {
        throw new System.Exception("Cannot set UsedAccessTerminal to false");
      }
    }
  }

  private static string SceneName => SceneManager.GetActiveScene().name;
  private static string GotKeycardKey => SceneName + ":GotKeycard";
  private static string UsedAccessTerminalKey => SceneName + ":UsedAccessTerminal";
}
