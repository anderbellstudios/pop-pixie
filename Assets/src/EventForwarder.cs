using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Event handlers between two unrelated game objects can be difficult to
 * locate in the editor, and can be difficult to maintain if object A needs to
 * send events to multiple objects B, C and D. Use EventForwarder to give this
 * event a meaningful name and make it discoverable in the hierarchy.
 */
public class EventForwarder : MonoBehaviour {
  public bool InvokeOnAwake, InvokeOnStart;
  public float Delay = 0f;
  public bool DelayUsesPlayingTime;
  public UnityEvent Events;

  void Awake() {
    if (InvokeOnAwake)
      Invoke();
  }

  void Start() {
    if (InvokeOnStart)
      Invoke();
  }

  public void Invoke() {
    if (Delay == 0f) {
      Events.Invoke();
    } else if (DelayUsesPlayingTime) {
      AsyncTimer.PlayingTime.SetTimeout(Events.Invoke, Delay);
    } else {
      AsyncTimer.BaseTime.SetTimeout(Events.Invoke, Delay);
    }
  }
}
