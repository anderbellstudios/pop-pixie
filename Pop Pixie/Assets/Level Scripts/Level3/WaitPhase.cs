using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitPhase : APhase {

  [SerializeField] public UnityEvent OnBegin;

	public override void LocalBegin () {
    OnBegin.Invoke();
  }

  public void StopWaiting() {
    if (Running)
      PhaseFinished();
  }

}
