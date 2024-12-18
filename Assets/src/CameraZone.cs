using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour {
  public CameraState CameraState;
  public float Duration;

  void OnTriggerEnter2D(Collider2D collider) {
    if (collider.tag == "Player") {
      CameraManager.Current.AnimateTo(CameraState, Duration);
    }
  }

  void OnTriggerExit2D(Collider2D collider) {
    if (collider.tag == "Player") {
      CameraManager.Current.Reset(Duration);
    }
  }
}
