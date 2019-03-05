using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreItemSprite : MonoBehaviour {

  public LoreManager Lore;
  public string LoreItemResourceName;
  public DialoguePromptManager PromptManager;

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      PromptManager.Display();
      // Lore.Open("Lore/" + LoreItemResourceName);
    }
  }
}
