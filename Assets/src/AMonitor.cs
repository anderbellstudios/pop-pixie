using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class AMonitor : MonoBehaviour {
  public bool WaitOnAwake = true, WaitInfinitely = false, Waiting;

  public UnityEvent Event;

  public void StartWaiting() {
    Waiting = true;
    BeganWaiting();
  }

  public virtual void BeganWaiting() { }

  void Awake() {
    if (WaitOnAwake)
      Waiting = true;

    LocalAwake();
  }

  public virtual void LocalAwake() { }

  void Update() {
    if (Waiting && TestCondition()) {
      ConditionMet();
    }

    LocalUpdate();
  }

  public void ConditionMet() {
    if (!WaitInfinitely)
      Waiting = false;

    Event.Invoke();
  }

  public virtual void LocalUpdate() { }

  public virtual bool TestCondition() {
    return false;
  }
}
