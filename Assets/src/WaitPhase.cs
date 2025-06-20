using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitPhase : APhase {
  public UnityEvent OnBegin;

  public override void LocalBegin() {
    OnBegin.Invoke();
  }

  public void StopWaiting() {
    if (Running)
      PhaseFinished();
  }
}
