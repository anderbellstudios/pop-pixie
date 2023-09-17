using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToFacePlayer : MonoBehaviour {
  void Update() {
    if (!StateManager.Playing)
      return;

    Vector3 toPlayer = PlayerGameObject.Current.transform.position - transform.position;

    transform.rotation = Quaternion.Euler(
      0,
      0,
      Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg
    );
  }
}
