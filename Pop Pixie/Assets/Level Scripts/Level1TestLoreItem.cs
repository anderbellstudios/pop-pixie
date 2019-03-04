using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1TestLoreItem : MonoBehaviour {

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      Debug.Log("And now, lore happens!");
    }
  }
}
