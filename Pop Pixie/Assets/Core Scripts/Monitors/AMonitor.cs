using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class AMonitor : MonoBehaviour, ISerializableComponent {
  public string[] SerializableFields { get; } = { "Waiting" };

  public bool WaitOnAwake = true, WaitInfinitely = false, Waiting;

  [SerializeField] public UnityEvent Event;

  public void StartWaiting() {
    Waiting = true;
  }

  void Awake() {
    if (WaitOnAwake)
      Waiting = true;

    LocalAwake();
  }

  public virtual void LocalAwake() {}

  void Update() {
    if (Waiting && TestCondition()) {
      if (!WaitInfinitely)
        Waiting = false;

      Event.Invoke();
    }

    LocalUpdate();
  }

  public virtual void LocalUpdate() {}

  public abstract bool TestCondition();
}
