using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationPrompt : MonoBehaviour
{
  /*
   Plan:
  NotificationPrompt (rename to ObjectivesNotification, ObjectivesNotificationPrompt ?
  shows current object, or just completed objective with new objective
  - when inactive for a while, show objective
  - when active again, hide
  - completed objective/new object show again

  create another class to store all objectives
  - if a lot of objectives, might need button to display them all on menu
   
   */

  // Lower entries take precedence
  public enum Priority {
    CollectIntel,
  }

  public TMP_Text Text;

  public bool SingletonInstance = true;
  public static NotificationPrompt Current;

  public delegate String NotificationPromptSource();

  public float TimeInactiveUntilShowObjective;
  public float TimeUntilHideObjectiveOnceActive;

  private readonly List<(Int32, NotificationPromptSource)> Sources = new();

  void Awake() {
    if (SingletonInstance)
      Current = this;
  }

  public void RegisterSource(Priority priority, NotificationPromptSource source) {
    Sources.Add(((int)priority, source));
  }
  void Update(){
    Text.text = CurrentText();
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
