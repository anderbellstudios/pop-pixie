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
    public bool IsBoundToGameObject;
    public GameObject BoundGameObject;
    public System.Action Callback;

    // Return true if the event should be removed from the queue
    public bool Update(float currentTime) {
      if (IsBoundToGameObject && BoundGameObject == null)
        return true;

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
    /**
     * A previous version of this code used EnqueuedEvents.RemoveAll. This was
     * causing problems since RemoveAll doesn't expect the list to be mutated
     * by the predicate, causing the wrong EnqueuedEvent to be removed in some
     * circumstances.
     */
    List<EnqueuedEvent> enqueuedEvents = new List<EnqueuedEvent>(EnqueuedEvents);
    enqueuedEvents.ForEach(enqueuedEvent => {
      if (enqueuedEvent.Update(currentTime)) {
        EnqueuedEvents.Remove(enqueuedEvent);
      }
    });
  }

  public EnqueuedEvent SetTimeout(System.Action callback, float timeout, GameObject bindToGameObject = null) {
    EnqueuedEvent enqueuedEvent = new EnqueuedEvent() {
      Repeating = false,
      Time = CurrentTime + timeout,
      Interval = null,
      Callback = callback,
      IsBoundToGameObject = bindToGameObject != null,
      BoundGameObject = bindToGameObject
    };
    EnqueuedEvents.Add(enqueuedEvent);
    return enqueuedEvent;
  }

  public EnqueuedEvent SetInterval(System.Action callback, float interval, GameObject bindToGameObject = null) {
    EnqueuedEvent enqueuedEvent = new EnqueuedEvent() {
      Repeating = true,
      Time = CurrentTime + interval,
      Interval = interval,
      Callback = callback,
      IsBoundToGameObject = bindToGameObject != null,
      BoundGameObject = bindToGameObject
    };
    EnqueuedEvents.Add(enqueuedEvent);
    return enqueuedEvent;
  }

  public void ClearTimeout(EnqueuedEvent enqueuedEvent) {
    EnqueuedEvents.Remove(enqueuedEvent);
  }
}
