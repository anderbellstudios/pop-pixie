using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePrompt : MonoBehaviour {
  // Lower entries take precedence
  public enum Priority {
    TutorialRoll,
    TutorialFire,
    TutorialMove,
    UrgentRoll,
    Uncrouch,
    Reload,
    Inspect,
  }

  public delegate String InGamePromptSource();

  public bool SingletonInstance = true;
  public static InGamePrompt Current;

  public TMP_Text Text;

  private LowPriorityBehaviour LowPriorityBehaviour;
  private List<(Int32, InGamePromptSource)> Sources = new List<(Int32, InGamePromptSource)>();

  void Awake() {
    if (SingletonInstance)
      Current = this;

    LowPriorityBehaviour = new LowPriorityBehaviour();
  }

  public void RegisterSource(Priority priority, InGamePromptSource source) {
    Sources.Add(((int)priority, source));
  }

  void Update() {
    LowPriorityBehaviour.EveryNFrames(
      10,
      () => {
        Text.text = CurrentText();
      }
    );
  }

  String CurrentText() {
    if (!StateManager.Playing)
      return null;

    String resultingText = null;
    int highestPriority = 0;

    foreach ((int priority, InGamePromptSource source) sourceWithPriority in Sources) {
      String text = sourceWithPriority.source();
      int priority = sourceWithPriority.priority;

      if (text != null && (resultingText == null || priority >= highestPriority)) {
        resultingText = text;
        highestPriority = priority;
      }
    }

    return resultingText;
  }
}
