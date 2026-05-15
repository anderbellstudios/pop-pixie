using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationPrompt : MonoBehaviour
{
  public TMP_Text Text;

  public bool SingletonInstance = true;
  public static NotificationPrompt Current;

  public delegate String NotificationPromptSource(); 


  void Awake() {
    if (SingletonInstance)
      Current = this;
  }

  private string currentText = "empty text";
  public void SetNotificationPrompt(NotificationPromptSource source) {
    String text = source();
    if(text != null) {
      currentText = text;
    } else {
      Debug.Log("text is null");
    }
  }
  void Update(){
    Text.text = currentText;
  }
}
