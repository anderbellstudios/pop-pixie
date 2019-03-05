using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1TestLoreItem : MonoBehaviour {

  public LoreManager Lore;

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      Lore.Open("Lore/l1l1");
    }
  }
}
