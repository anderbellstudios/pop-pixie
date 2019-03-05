using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreItemSprite : MonoBehaviour, IPromptButtonEventHandler {

  public LoreManager Lore;
  public string LoreItemResourceName;
  public string PromptText;
  public DialoguePromptManager PromptManager;

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      PromptManager.Display(
        PromptText + "\n(Do you read it?)",
        "Read it",
        "Do not",
        this
      );
    }
  }

  public void ButtonPressed (string button) {
    if ( button == "positive" ) {
      Lore.Open("Lore/" + LoreItemResourceName);
    }
  }
}
