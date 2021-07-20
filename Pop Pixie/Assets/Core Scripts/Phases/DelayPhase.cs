using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPhase : APhase {

  public bool PauseGameplay;
  public float Delay;

	public override void LocalBegin () {
    if (PauseGameplay)
      StateManager.SetState( State.Cutscene );

    Invoke("AfterDelay", Delay);
  }

  void AfterDelay() {
    if (PauseGameplay)
      StateManager.SetState( State.Playing );

    PhaseFinished();
  }

}
