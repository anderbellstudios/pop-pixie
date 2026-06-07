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
  shows current objective, or just completed objective crossed out with new objective underneath
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

  private string CurrentObjectiveText;
  public void SetNewObjective(string newObjective) {
    Debug.Log("set new objective to: " + newObjective);
    CurrentObjectiveText = newObjective;
  }

  void Awake() {
    if (SingletonInstance)
      Current = this;
    CurrentObjectiveText = "This is a placeholder objective";
  }

  void Update(){
    //check if inactive for period of time
    //if so show objective
    //if not hide objective
    //take a second to hide objective after becoming active

    if (!StateManager.Playing) {
      return;
    }

    if (PlayingTime.time - PlayerGameObject.LastMovedAt > TimeInactiveUntilShowObjective) {
      Text.text = GetCurrentObjective();
    } 
    else {
      Text.text = null;
    }

    
  }

  private string GetCurrentObjective() {
    return CurrentObjectiveText;
  }

  
}
