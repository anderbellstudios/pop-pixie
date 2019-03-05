using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreItemSprite : MonoBehaviour {

  public LoreManager Lore;
  public string LoreItemResourceName;

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      Lore.Open("Lore/" + LoreItemResourceName);
    }
  }
}
