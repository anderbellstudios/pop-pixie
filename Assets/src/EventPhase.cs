using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventPhase : APhase {

  [SerializeField] public UnityEvent Event;

  public override void LocalBegin() {
    Event.Invoke();
    PhaseFinished();
  }

}
