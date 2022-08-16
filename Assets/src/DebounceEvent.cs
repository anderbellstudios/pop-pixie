using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebounceEvent : MonoBehaviour {
  public UnityEvent Event;
  public float Cooldown;
  public bool UsePlayingTime = true;

  private float LastTime = float.MinValue;

  public void Invoke() {
    float time = UsePlayingTime ? PlayingTime.time : Time.time;

    if (time - LastTime > Cooldown) {
      LastTime = time;
      Event.Invoke();
    }
  }
}
