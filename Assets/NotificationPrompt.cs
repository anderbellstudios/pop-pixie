using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationPrompt : MonoBehaviour
{
  // Lower entries take precedence
  public enum Priority {
    CollectIntel,
  }

  public TMP_Text Text;

  public bool SingletonInstance = true;
  public static NotificationPrompt Current;

  public delegate String NotificationPromptSource();

  private LowPriorityBehaviour LowPriorityBehaviour;
  private readonly List<(Int32, NotificationPromptSource)> Sources = new();

  void Awake() {
    if (SingletonInstance)
      Current = this;
    LowPriorityBehaviour = new LowPriorityBehaviour();
  }

  public void RegisterSource(Priority priority, NotificationPromptSource source) {
    Sources.Add(((int)priority, source));
  }
  void Update(){
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

    foreach ((int priority, NotificationPromptSource source) sourceWithPriority in Sources) {
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
