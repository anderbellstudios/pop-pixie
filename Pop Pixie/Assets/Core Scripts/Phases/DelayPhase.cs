using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPhase : APhase {

  public bool PauseGameplay;
  public float Delay;

	public override void LocalBegin () {
    if (PauseGameplay)
      StateManager.AddState(State.NotPlaying);

    Invoke("AfterDelay", Delay);
  }

  void AfterDelay() {
    if (PauseGameplay)
      StateManager.RemoveState(State.NotPlaying);

    PhaseFinished();
  }

}
