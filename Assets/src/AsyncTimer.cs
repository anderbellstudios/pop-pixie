using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AsyncTimer : MonoBehaviour {
  public static AsyncTimerBaseTime BaseTime;
  public static AsyncTimerPlayingTime PlayingTime;

  public class EnqueuedEvent {
    public bool Repeating;
    public float Time;
    public float? Interval;
    public System.Action Callback;

    public EnqueuedEvent(bool repeating, float time, float? interval, System.Action callback) {
      Repeating = repeating;
      Time = time;
      Interval = interval;
      Callback = callback;
    }

    // Return true if the event should be removed from the queue
    public bool Update(float currentTime) {
      if (Time > currentTime)
        return false;

      Callback();

      if (Repeating) {
        Time += Interval.Value;
        return false;
      } else {
        return true;
      }
    }
  }

  public bool SingletonInstance = true;

  List<EnqueuedEvent> EnqueuedEvents = new List<EnqueuedEvent>();

  public abstract float CurrentTime { get; }
  public abstract void SetAsSingleton();

  void Awake() {
    if (SingletonInstance)
      SetAsSingleton();
  }

  void Update() {
    float currentTime = CurrentTime;
    EnqueuedEvents.RemoveAll(enqueuedEvent => enqueuedEvent.Update(currentTime));
  }

  public EnqueuedEvent SetTimeout(System.Action callback, float timeout) {
    EnqueuedEvent enqueuedEvent = new EnqueuedEvent(false, CurrentTime + timeout, null, callback);
    EnqueuedEvents.Add(enqueuedEvent);
    return enqueuedEvent;
  }

  public EnqueuedEvent SetInterval(System.Action callback, float interval) {
    EnqueuedEvent enqueuedEvent = new EnqueuedEvent(true, CurrentTime + interval, interval, callback);
    EnqueuedEvents.Add(enqueuedEvent);
    return enqueuedEvent;
  }

  public void ClearTimeout(EnqueuedEvent enqueuedEvent) {
    EnqueuedEvents.Remove(enqueuedEvent);
  }
}
