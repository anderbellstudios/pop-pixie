using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInCrouchZone : MonoBehaviour {
  void OnTriggerEnter2D(Collider2D collider) {
    if (collider.tag == "Player") {
      Crouch.Current.SetInCrouchZone(true);
    }
  }

  void OnTriggerExit2D(Collider2D collider) {
    if (collider.tag == "Player") {
      Crouch.Current.SetInCrouchZone(false);
    }
  }
}
