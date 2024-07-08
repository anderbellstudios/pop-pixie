using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PhaseFinishedDelegate();

public abstract class APhase : MonoBehaviour {
  private PhaseFinishedDelegate _FinishedCallback;
  public bool Running = false;

  public void Begin(PhaseFinishedDelegate finishedCallback) {
    _FinishedCallback = finishedCallback;

    if (SceneEvents.Current.IsRetry && SkipOnRetry()) {
      PhaseFinished();
    } else {
      Running = true;
      LocalBegin();
    }
  }

  public virtual bool SkipOnRetry() {
    return false;
  }

  public virtual void LocalBegin() {
  }

  public virtual void Update() {
    if (Running)
      WhilePhaseRunning();
  }

  public virtual void WhilePhaseRunning() {
  }

  public void PhaseFinished() {
    Running = false;
    AfterFinished();
    _FinishedCallback();
  }

  public virtual void AfterFinished() {
  }

  public virtual float ProgressBarAllotment() {
    return 0f;
  }

  public virtual float ProgressBarValue() {
    return 0f;
  }
}
