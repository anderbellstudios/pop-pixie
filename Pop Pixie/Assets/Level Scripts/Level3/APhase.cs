using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PhaseFinishedDelegate();

public abstract class APhase : MonoBehaviour {

  private PhaseFinishedDelegate _FinishedCallback;
  public bool PhaseRunning = false;

  public void Begin( PhaseFinishedDelegate finishedCallback ) {
    _FinishedCallback = finishedCallback;
    PhaseRunning = true;
    LocalBegin();
  }

  public virtual void LocalBegin() {
  }

  public virtual void Update() {
    if ( PhaseRunning )
      WhilePhaseRunning();
  }

  public virtual void WhilePhaseRunning() {
  }

  public void PhaseFinished() {
    PhaseRunning = false;
    AfterFinished();
    _FinishedCallback();
  }

  public virtual void AfterFinished() {
  }
}
