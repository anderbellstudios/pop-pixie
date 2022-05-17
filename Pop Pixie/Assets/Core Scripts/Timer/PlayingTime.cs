using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingTime : MonoBehaviour {
  public static float time;

  void Update() {
    if (!StateManager.Playing)
      return;

    time += Time.deltaTime;
  }

}
